using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.Auth;
using WebAPI.Data;
using WebAPI.DTOs.Actor;
using WebAPI.DTOs.Genre;
using WebAPI.DTOs.Movie;
using WebAPI.DTOs.Review;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public MovieService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
		}
        public async Task<MovieResponseDTO> CreateMovieAsync(CreateMovieDTO request, string userId)
        {
            var movie = new Movie()
            {
                Title = request.Title,
                ReleaseDate = request.ReleaseDate,
                Rating = request.Rating,
                UserId = userId
            };

            if (request.ActorIds.Any())
            {
                var actors = await _context.Actors
                    .Where(e => request.ActorIds.Contains(e.Id))
                    .ToListAsync();
                
                movie.Actors = actors;
            }

            if(request.GenreIds.Any())
            {
                var genres = await _context.Genres
                    .Where(e => request.GenreIds.Contains(e.Id))
                    .ToListAsync();

                movie.Genres = genres;
            }

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return new MovieResponseDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                UserId = movie.UserId
            };
        }

		public async Task<bool> DeleteMovieAsync(int id, string userId)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(e => e.Id == id);

            if (movie == null)
            {
                return false;
            }

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return false;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (movie.UserId != userId && !isAdmin)
			{
				return false;
			}

			_context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<MovieResponseDTO?> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(e => e.Id == id);

            if (movie == null)
            {
                return null;
            }

            return new MovieResponseDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                UserId = movie.UserId
            };
        }

        public async Task<MovieResponseDTO?> UpdateMovieAsync(UpdateMovieDTO request, string userId)
        {
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(e => e.Id == request.Id);

            if (movie == null)
            {
                return null;
            }

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return null;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (movie.UserId != userId && !isAdmin)
			{
				return null;
			}

			movie.Title = request.Title;
            movie.ReleaseDate = request.ReleaseDate;
            movie.Rating = request.Rating;

            if (request.ActorIds != null && request.ActorIds.Any())
            {
                var actors = await _context.Actors
                    .Where(a => request.ActorIds.Contains(a.Id))
                    .ToListAsync();

                movie.Actors = actors;
            }
            else if (request.ActorIds != null)
            {
                movie.Actors?.Clear();
            }

            if (request.GenreIds != null && request.GenreIds.Any())
            {
                var genres = await _context.Genres
                    .Where(g => request.GenreIds.Contains(g.Id))
                    .ToListAsync();

                movie.Genres = genres;
            }
            else if (request.GenreIds != null)
            {
                movie.Genres?.Clear();
            }

            await _context.SaveChangesAsync();

            return new MovieResponseDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                UserId = movie.UserId
            };
        }

        public async Task<List<GenreResponseDTO>> GetAllGenresByMovieId(int id)
        {
            var genres = _context.Genres
                .Include(g => g.Movies)
                .AsEnumerable()
                .Where(g => g.Movies != null && g.Movies.Any(m => m.Id == id))
                .ToList();

            //var genres = await _context.Genres
            //    .Where(g => g.Movies != null && g.Movies.Any(m => m.Id == id))
            //    .ToListAsync();

            var result = genres.Select(g => new GenreResponseDTO
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                UserId = g.UserId
            }).ToList();

            return result;
        }

        public async Task<List<ActorResponseDTO>> GetAllActorsByMovieId(int id)
        {
            var actors = _context.Actors
                .Include(e => e.Movies)
                .AsEnumerable()
                .Where(e => e.Movies != null && e.Movies.Any(e => e.Id == id))
                .ToList();

            var result = actors.Select(e => new ActorResponseDTO
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                BirthDate = e.BirthDate,
                Bio = e.Bio,
                UserId = e.UserId
            }).ToList();

            return result;
        }

        public async Task<List<ReviewResponseDTO>> GetAllReviewsByMovieId(int id)
        {
            var reviews = await _context.Reviews
                .Include(e => e.Movie)
                .Where(e => e.MovieId == id)
                .ToListAsync();

            var result = reviews.Select(e => new ReviewResponseDTO
            {
                Id = e.Id,
                Author = e.Author,
                Content = e.Content,
                Rating = e.Rating,
                CreatedAt = e.CreatedAt,
                UserId = e.UserId
            }).ToList();

            return result;
        }

        public async Task<List<MovieResponseDTO>> GetAllMovies()
        {
            var movies = await _context.Movies
                .Include(e => e.Actors)
                .Include(e => e.Genres)
				.AsNoTracking()
                .ToListAsync();

            var result = movies.Select(e => new MovieResponseDTO
            {
                Id = e.Id,
                Rating = e.Rating,
                ReleaseDate = e.ReleaseDate,
                Title = e.Title,
                UserId = e.UserId,
                Actors = e.Actors?.Select(a => new ActorResponseDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    BirthDate = a.BirthDate,
                    Bio = a.Bio
                }).ToList(),
                Genres = e.Genres?.Select(g => new GenreResponseDTO
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description
                }).ToList()
			}).ToList();

            return result;
        }

		public async Task<MovieResponseDTO?> UpdateMovieOnlyAsync(UpdateMovieDTO request, string userId)
		{
			var movie = await _context.Movies
				.FirstOrDefaultAsync(e => e.Id == request.Id);

			if (movie == null)
			{
				return null;
			}

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return null;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (movie.UserId != userId && !isAdmin)
			{
				return null;
			}

			movie.Title = request.Title;
			movie.ReleaseDate = request.ReleaseDate;
			movie.Rating = request.Rating;

			await _context.SaveChangesAsync();

			return new MovieResponseDTO()
			{
				Id = movie.Id,
				Title = movie.Title,
				ReleaseDate = movie.ReleaseDate,
				Rating = movie.Rating,
                UserId = movie.UserId
			};
		}

		public async Task<MovieResponseDTO> CreateMovieOnlyAsync(CreateMovieDTO request, string userId)
		{
			var movie = new Movie()
			{
				Title = request.Title,
				ReleaseDate = request.ReleaseDate,
				Rating = request.Rating,
				UserId = userId
			};

			if (request.GenreIds.Any())
			{
				var genres = await _context.Genres
					.Where(e => request.GenreIds.Contains(e.Id))
					.ToListAsync();

				movie.Genres = genres;
			}

			await _context.Movies.AddAsync(movie);
			await _context.SaveChangesAsync();

			return new MovieResponseDTO()
			{
				Id = movie.Id,
				Title = movie.Title,
				ReleaseDate = movie.ReleaseDate,
				Rating = movie.Rating,
                UserId = movie.UserId
			};
		}
		public async Task<MovieResponseDTO?> AddActorToMovieAsync(int movieId, int actorId, string userId)
		{
			var movie = await _context.Movies
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == movieId && m.UserId == userId);

			if (movie == null)
				return null;

			var actor = await _context.Actors.FindAsync(actorId);
			if (actor == null)
				return null;

			if (!movie.Actors.Any(a => a.Id == actorId))
			{
				movie.Actors.Add(actor);
				await _context.SaveChangesAsync();
			}

			return new MovieResponseDTO
			{
				Id = movie.Id,
				Title = movie.Title,
				ReleaseDate = movie.ReleaseDate,
				Rating = movie.Rating,
                UserId = movie.UserId
			};
		}

		public async Task<bool> RemoveActorFromMovieAsync(int movieId, int actorId, string userId)
		{
			var movie = await _context.Movies
				.Include(m => m.Actors)
				.FirstOrDefaultAsync(m => m.Id == movieId && m.UserId == userId);

			if (movie == null)
				return false;

			var actor = movie.Actors.FirstOrDefault(a => a.Id == actorId);
			if (actor == null)
				return false;

			movie.Actors.Remove(actor);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
