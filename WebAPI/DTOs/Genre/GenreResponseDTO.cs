namespace WebAPI.DTOs.Genre
{
    public class GenreResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
	}
}
