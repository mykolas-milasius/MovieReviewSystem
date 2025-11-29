using WebAPI.DTOs.Actor;

namespace WebAPI.Interfaces
{
    public interface IActorService
    {
        Task<ActorResponseDTO> CreateActorAsync(CreateActorDTO request, string userId);
        Task<ActorResponseDTO?> UpdateActorAsync(UpdateActorDTO request, string userId));
        Task<ActorResponseDTO?> GetActorByIdAsync(int id);
        Task<bool> DeleteActorAsync(int id, string userId));
        Task<List<ActorResponseDTO>> GetAllActorsAsync();
	}
}
