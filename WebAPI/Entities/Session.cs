using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
	public class Session
	{
		public Guid Id { get; set; }
		public string LastRefreshToken { get; set; } = string.Empty;
		public DateTimeOffset InitiatedAt { get; set; } 
		public DateTimeOffset ExpiresAt { get; set; }
		public bool IsRevoked { get; set; }

		[Required]
		public required string UserId { get; set; }
		public User User { get; set; }
	}
}
