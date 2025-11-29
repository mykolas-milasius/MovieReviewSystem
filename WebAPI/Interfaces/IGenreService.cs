
using WebAPI.DTOs.Genre;

namespace WebAPI.Interfaces
{
    public interface IGenreService
    {
        Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO request, string userId);
        Task<GenreResponseDTO?> UpdateGenreAsync(UpdateGenreDTO request, string userId);
        Task<GenreResponseDTO?> GetGenreByIdAsync(int id);
        Task<bool> DeleteGenreAsync(int id, string userId);
        Task<List<GenreResponseDTO>> GetAllAsync();
    }
}
