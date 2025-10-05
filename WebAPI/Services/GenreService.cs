using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTOs.Genre;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;

        public GenreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO request)
        {
            var result = await _context.Genres.AddAsync(new Genre()
            {
                Title = request.Title,
                Description = request.Description,
            });

            await _context.SaveChangesAsync();

            return new GenreResponseDTO()
            {
                Id = result.Entity.Id,
                Title = result.Entity.Title,
                Description = result.Entity.Description,
            };
        }

        public async Task<bool> DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(e => e.Id == id);

            if (genre == null)
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

        public async Task<GenreResponseDTO?> UpdateGenreAsync(UpdateGenreDTO request)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(e => e.Id == request.Id);

            if (genre == null)
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
    }
}
