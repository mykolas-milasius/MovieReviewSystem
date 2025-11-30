using WebAPI.DTOs.Actor;
using WebAPI.DTOs.Genre;

namespace WebAPI.DTOs.Movie
{
    public class MovieResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }
        public double Rating { get; set; }
        public List<GenreResponseDTO>? Genres { get; set; }
        public List<ActorResponseDTO>? Actors { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
