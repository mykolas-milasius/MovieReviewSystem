using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Auth;
using WebAPI.Data;
using WebAPI.DTOs.Actor;
using WebAPI.DTOs.Genre;
using WebAPI.DTOs.Movie;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public GenreService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
		}

        public async Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO request, string userId)
        {
            var result = await _context.Genres.AddAsync(new Genre()
            {
                Title = request.Title,
                Description = request.Description,
                UserId = userId,
			});

            await _context.SaveChangesAsync();

            return new GenreResponseDTO()
            {
                Id = result.Entity.Id,
                Title = result.Entity.Title,
                Description = result.Entity.Description,
            };
        }

        public async Task<bool> DeleteGenreAsync(int id, string userId)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(e => e.Id == id);

            if (genre == null)
            {
                return false;
            }

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return false;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (genre.UserId != userId && !isAdmin)
			{
				return false;
			}

			_context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<GenreResponseDTO>> GetAllAsync()
        {
            var genres = await _context.Genres.ToListAsync();

            var result = genres.Select(g => new GenreResponseDTO
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
            }).ToList();

            return result;
        }

        public async Task<GenreResponseDTO?> GetGenreByIdAsync(int id)
        {
            var result = await _context.Genres.FirstOrDefaultAsync(e => e.Id == id);

            if(result == null)
            {
                return null;
            }

            return new GenreResponseDTO()
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
            };
        }

        public async Task<GenreResponseDTO?> UpdateGenreAsync(UpdateGenreDTO request, string userId)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(e => e.Id == request.Id);

            if (genre == null)
            {
                return null;
            }

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return null;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (genre.UserId != userId && !isAdmin)
			{
				return null;
			}

			genre.Title = request.Title;
            genre.Description = request.Description;

            await _context.SaveChangesAsync();

            return new GenreResponseDTO()
            {
                Id = genre.Id,
                Title = genre.Title,
                Description = genre.Description,
            };
        }

        public async Task<List<MovieResponseDTO>> GetMoviesByGenreIdAsync(int genreId)
        {
            var genre = await _context.Genres
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.Id == genreId);

            if (genre == null || !genre.Movies.Any())
            {
                return new List<MovieResponseDTO>();
            }

            return genre.Movies.Select(m => new MovieResponseDTO
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                Rating = m.Rating,
                Genres = m.Genres?.Select(g => new GenreResponseDTO
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                }).ToList(),
                Actors = m.Actors?.Select(a => new ActorResponseDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
					Bio = a.Bio,
                }).ToList(),
            }).ToList();
		}
	}
}
