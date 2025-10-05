namespace WebAPI.DTOs.Movie
{
    public class MovieResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }
        public double Rating { get; set; }
    }
}
