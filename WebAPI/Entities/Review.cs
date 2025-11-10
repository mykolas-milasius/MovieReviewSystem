using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MovieId { get; set; }
        [Required]
        public string UserId { get; set; }
        // Navigational
        public Movie? Movie { get; set; }
        public User? User { get; set; }
    }
}
