using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTOs.Review;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ReviewResponseDTO> CreateReviewAsync(CreateReviewDTO request)
        {
            var result = await _context.Reviews.AddAsync(new Review()
            {
                Author = request.Author,
                Content = request.Content,
                Rating = request.Rating,
                CreatedAt = request.CreatedAt,
                MovieId = request.MovieId
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

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(e => e.Id == id);

            if (review == null)
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

        public async Task<ReviewResponseDTO?> UpdateReviewAsync(UpdateReviewDTO request)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(e => e.Id == request.Id);

            if (review == null)
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
