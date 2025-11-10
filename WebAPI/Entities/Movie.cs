using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }
        public double Rating { get; set; }
        [Required]
        public string UserId { get; set; }

        // Navigational
        public ICollection<Genre>? Genres { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Actor>? Actors { get; set; }
        public User? User { get; set; }
    }
}
