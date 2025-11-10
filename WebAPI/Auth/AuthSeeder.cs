
using Microsoft.AspNetCore.Identity;
using WebAPI.Entities;

namespace WebAPI.Auth
{
    public class AuthSeeder
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRolesAsync();
            await AddAdminUserAsync();
        }

        private async Task AddAdminUserAsync()
        {
            var newAdminUser = new User()
            {
                UserName = "admin",
                Email = "admin@admin.lt",
            };

            var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);

            if(existingAdminUser == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "Admin123.");

                if(createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }

        private async Task AddDefaultRolesAsync()
        {
            foreach (var role in UserRoles.AllRoles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
