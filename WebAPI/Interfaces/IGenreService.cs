
using WebAPI.DTOs.Genre;

namespace WebAPI.Interfaces
{
    public interface IGenreService
    {
        Task<GenreResponseDTO> CreateGenreAsync(CreateGenreDTO request);
        Task<GenreResponseDTO?> UpdateGenreAsync(UpdateGenreDTO request);
        Task<GenreResponseDTO?> GetGenreByIdAsync(int id);
        Task<bool> DeleteGenreAsync(int id);
        Task<List<GenreResponseDTO>> GetAllAsync();
    }
}
