using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;

namespace WebAPI.Auth
{
    public static class AuthEndpoints
    {
        public static void AddAuthApi(this WebApplication app)
        {
            // register

            app.MapPost("api/accounts", async (UserManager<User> userManager, AppDbContext context, RegisterUserDTO dto) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);

                if (user != null)
                {
                    return Results.UnprocessableEntity(error: "Username already taken");
                }

                var newUser = new User()
                {
                    Email = dto.Email,
                    UserName = dto.UserName,
                };


                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    var createUserResult = await userManager.CreateAsync(newUser, dto.Password);
                    if (!createUserResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return Results.UnprocessableEntity();
                    }

                    await userManager.AddToRoleAsync(newUser, UserRoles.User);

                    await transaction.CommitAsync();
                    return Results.Created($"/api/users/{newUser.Id}", newUser);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                //var createUserResult = await userManager.CreateAsync(newUser, dto.Password);

                //if(!createUserResult.Succeeded)
                //{
                //    return Results.UnprocessableEntity();
                //}

                //await userManager.AddToRoleAsync(newUser, UserRoles.User);
            });

            //login

            app.MapPost("api/login", async (UserManager<User> userManager, JwtTokenService jwtTokenService, LoginDTO dto) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);

                if (user == null)
                {
                    return Results.UnprocessableEntity(error: "User does not exist");
                }

                var isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);

                if (!isPasswordValid)
                {
                    return Results.UnprocessableEntity(error: "UserName or Password is invalid");
                }

                var roles = await userManager.GetRolesAsync(user);

                var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

                return Results.Ok(new SuccessfullLoginDTO(accessToken));
            });
        }
    }
    public record RegisterUserDTO(string UserName, string Email,string Password);
    public record LoginDTO(string UserName, string Password);
    public record SuccessfullLoginDTO(string AccessToken);
}
