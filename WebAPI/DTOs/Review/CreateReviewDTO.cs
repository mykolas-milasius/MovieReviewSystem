namespace WebAPI.DTOs.Review
{
    public class CreateReviewDTO
    {
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MovieId { get; set; }
    }
}
