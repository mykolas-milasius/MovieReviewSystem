namespace WebAPI.DTOs.Review
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MovieId { get; set; }
		public string UserId { get; set; } = string.Empty;
	}
}
