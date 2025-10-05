using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTOs.Actor;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class ActorService : IActorService
    {
        private readonly AppDbContext _context;

        public ActorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActorResponseDTO> CreateActorAsync(CreateActorDTO request)
        {
            var result = await _context.Actors.AddAsync(new Actor()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Bio = request.Bio,
            });

            await _context.SaveChangesAsync();

            return new ActorResponseDTO()
            {
                Id = result.Entity.Id,
                FirstName = result.Entity.FirstName,
                LastName = result.Entity.LastName,
                BirthDate = result.Entity.BirthDate,
                Bio = result.Entity.Bio,
            };
        }

        public async Task<bool> DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(e => e.Id == id);

            if (actor == null)
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
                BirthDate = result.BirthDate
            };
        }

        public async Task<ActorResponseDTO?> UpdateActorAsync(UpdateActorDTO request)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(e => e.Id == request.Id);

            if (actor == null)
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
                BirthDate = actor.BirthDate
            };
        }
    }
}
