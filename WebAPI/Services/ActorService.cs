using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebAPI.Auth;
using WebAPI.Data;
using WebAPI.DTOs.Actor;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class ActorService : IActorService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public ActorService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ActorResponseDTO> CreateActorAsync(CreateActorDTO request, string userId)
        {
			var result = await _context.Actors.AddAsync(new Actor()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Bio = request.Bio,
                UserId = userId
            });

            await _context.SaveChangesAsync();

            return new ActorResponseDTO()
            {
                Id = result.Entity.Id,
                FirstName = result.Entity.FirstName,
                LastName = result.Entity.LastName,
                BirthDate = result.Entity.BirthDate,
                Bio = result.Entity.Bio,
                UserId = result.Entity.UserId
            };
        }

        public async Task<bool> DeleteActorAsync(int id, string userId)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(e => e.Id == id);

            if (actor == null)
            {
                return false;
            }

			var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
				return false;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (actor.UserId != userId && !isAdmin)
			{
                return false;
			}

			_context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ActorResponseDTO?> GetActorByIdAsync(int id)
        {
            var result = await _context.Actors.FirstOrDefaultAsync(e => e.Id == id);

            if (result == null)
            {
                return null;
            }

            return new ActorResponseDTO()
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Bio = result.Bio,
                BirthDate = result.BirthDate,
                UserId = result.UserId
            };
        }

        public async Task<ActorResponseDTO?> UpdateActorAsync(UpdateActorDTO request, string userId)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(e => e.Id == request.Id);

            if (actor == null)
            {
                return null;
            }

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return null;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (actor.UserId != userId && !isAdmin)
			{
				return null;
			}

			actor.FirstName = request.FirstName;
            actor.LastName = request.LastName;
            actor.Bio = request.Bio;
            actor.BirthDate = request.BirthDate;

            await _context.SaveChangesAsync();

            return new ActorResponseDTO()
            {
                Id = actor.Id,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                Bio = actor.Bio,
                BirthDate = actor.BirthDate,
				UserId = actor.UserId
			};
        }

        public async Task<List<ActorResponseDTO>> GetAllActorsAsync()
        {
            return await _context.Actors
                .Select(actor => new ActorResponseDTO()
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    BirthDate = actor.BirthDate,
                    Bio = actor.Bio,
                    UserId = actor.UserId
                })
                .ToListAsync();
		}
	}
}
