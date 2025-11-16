
using WebAPI.DTOs.Actor;
using WebAPI.DTOs.Genre;
using WebAPI.DTOs.Movie;
using WebAPI.DTOs.Review;

namespace WebAPI.Interfaces
{
    public interface IMovieService
    {
        Task<MovieResponseDTO> CreateMovieAsync(CreateMovieDTO request, HttpContext httpContext);
        Task<MovieResponseDTO?> UpdateMovieAsync(UpdateMovieDTO request);
        Task<MovieResponseDTO?> GetMovieByIdAsync(int id);
        Task<bool> DeleteMovieAsync(int id);
        Task<List<GenreResponseDTO>> GetAllGenresByMovieId(int id);
        Task<List<ActorResponseDTO>> GetAllActorsByMovieId(int id);
        Task<List<ReviewResponseDTO>> GetAllReviewsByMovieId(int id);
    }
}
