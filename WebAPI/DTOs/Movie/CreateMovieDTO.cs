namespace WebAPI.DTOs.Movie
{
    public class CreateMovieDTO
    {
        public string Title { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }
        public double Rating { get; set; }
        public List<int> ActorIds { get; set; } = new List<int>();
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}
