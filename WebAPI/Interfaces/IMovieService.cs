
using WebAPI.DTOs.Actor;
using WebAPI.DTOs.Genre;
using WebAPI.DTOs.Movie;
using WebAPI.DTOs.Review;

namespace WebAPI.Interfaces
{
    public interface IMovieService
    {
        Task<MovieResponseDTO> CreateMovieAsync(CreateMovieDTO request, string userId);
        Task<MovieResponseDTO?> UpdateMovieAsync(UpdateMovieDTO request, string userId);
        Task<MovieResponseDTO?> GetMovieByIdAsync(int id);
        Task<bool> DeleteMovieAsync(int id, string userId);
        Task<List<GenreResponseDTO>> GetAllGenresByMovieId(int id);
        Task<List<ActorResponseDTO>> GetAllActorsByMovieId(int id);
        Task<List<ReviewResponseDTO>> GetAllReviewsByMovieId(int id);
        Task<List<MovieResponseDTO>> GetAllMovies();
        Task<MovieResponseDTO?> UpdateMovieOnlyAsync(UpdateMovieDTO request, string userId);
        Task<MovieResponseDTO> CreateMovieOnlyAsync(CreateMovieDTO request, string userId);
		Task<MovieResponseDTO?> AddActorToMovieAsync(int movieId, int actorId, string userId);
		Task<bool> RemoveActorFromMovieAsync(int movieId, int actorId, string userId);
	}
}
