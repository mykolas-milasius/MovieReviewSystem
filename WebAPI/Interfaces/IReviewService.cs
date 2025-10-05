using WebAPI.DTOs.Actor;
using WebAPI.DTOs.Review;

namespace WebAPI.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO> CreateReviewAsync(CreateReviewDTO request);
        Task<ReviewResponseDTO?> UpdateReviewAsync(UpdateReviewDTO request);
        Task<ReviewResponseDTO?> GetReviewByIdAsync(int id);
        Task<bool> DeleteReviewAsync(int id);
    }
}
