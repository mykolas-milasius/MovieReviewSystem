using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs.Actor;
using WebAPI.DTOs.Review;

namespace WebAPI.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO> CreateReviewAsync(CreateReviewDTO request, string userId);
        Task<ReviewResponseDTO?> UpdateReviewAsync(UpdateReviewDTO request, string userId);
        Task<ReviewResponseDTO?> GetReviewByIdAsync(int id);
        Task<bool> DeleteReviewAsync(int id, string userId);
        Task<List<ReviewResponseDTO>> GetReviewsByGenreAndMovie(int genreId, int movieId);
        Task<List<ReviewResponseDTO>> GetAllAsync();
	}
}
