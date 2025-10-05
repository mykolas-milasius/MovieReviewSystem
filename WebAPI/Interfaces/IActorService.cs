using WebAPI.DTOs.Actor;

namespace WebAPI.Interfaces
{
    public interface IActorService
    {
        Task<ActorResponseDTO> CreateActorAsync(CreateActorDTO request);
        Task<ActorResponseDTO?> UpdateActorAsync(UpdateActorDTO request);
        Task<ActorResponseDTO?> GetActorByIdAsync(int id);
        Task<bool> DeleteActorAsync(int id);
    }
}
