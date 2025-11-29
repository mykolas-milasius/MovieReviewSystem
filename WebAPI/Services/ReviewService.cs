using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Auth;
using WebAPI.Data;
using WebAPI.DTOs.Review;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReviewService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
		}
        public async Task<ReviewResponseDTO> CreateReviewAsync(CreateReviewDTO request, string userId)
        {
            var result = await _context.Reviews.AddAsync(new Review()
            {
                Author = request.Author,
                Content = request.Content,
                Rating = request.Rating,
                CreatedAt = request.CreatedAt,
                MovieId = request.MovieId,
                UserId = userId
			});

            await _context.SaveChangesAsync();

            return new ReviewResponseDTO()
            {
                Id = result.Entity.Id,
                Author = result.Entity.Author,
                Content = result.Entity.Content,
                Rating = result.Entity.Rating,
                CreatedAt = result.Entity.CreatedAt,
                MovieId = result.Entity.MovieId
            };
        }

        public async Task<bool> DeleteReviewAsync(int id, string userId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(e => e.Id == id);

            if (review == null)
            {
                return false;
            }

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return false;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (review.UserId != userId && !isAdmin)
			{
				return false;
			}

			_context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ReviewResponseDTO?> GetReviewByIdAsync(int id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(e => e.Id == id);

            if(review == null)
            {
                return null;
            }

            return new ReviewResponseDTO()
            {
                Id = review.Id,
                Author = review.Author,
                Content = review.Content,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt,
                MovieId = review.MovieId
            };
        }

        public async Task<List<ReviewResponseDTO>> GetReviewsByGenreAndMovie(int genreId, int movieId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Movie)
                .ThenInclude(m => m.Genres)
                .Where(e => e.MovieId == movieId && e.Movie.Genres.Any(g => g.Id == genreId))
                .ToListAsync();

            return reviews.Select(review => new ReviewResponseDTO
            {
                Id = review.Id,
                Author = review.Author,
                Content = review.Content,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt,
                MovieId = review.MovieId
            }).ToList();
        }

        public async Task<ReviewResponseDTO?> UpdateReviewAsync(UpdateReviewDTO request, string userId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(e => e.Id == request.Id);

            if (review == null)
            {
                return null;
            }

			var user = await _context.Users.FindAsync(userId);

			if (user == null)
			{
				return null;
			}

			bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

			if (review.UserId != userId && !isAdmin)
			{
				return null;
			}

			review.Author = request.Author;
            review.Content = request.Content;
            review.Rating = request.Rating;
            //review.CreatedAt = request.CreatedAt;
            review.MovieId = request.MovieId;

            await _context.SaveChangesAsync();

            return new ReviewResponseDTO()
            {
                Id = review.Id,
                Author = review.Author,
                Content = review.Content,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt,
                MovieId = review.MovieId
            };
        }
    }
}
