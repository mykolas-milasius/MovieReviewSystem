using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;

namespace WebAPI.Auth
{
	public class SessionService(AppDbContext appDbContext)
	{
		public async Task CreateSessionAsync(Guid sessionId, string userId, string refreshToken, DateTime expiresAt)
		{
			appDbContext.Sessions.Add(new Entities.Session
			{
				Id = sessionId,
				UserId = userId,
				InitiatedAt = DateTimeOffset.UtcNow,
				ExpiresAt = expiresAt,
				LastRefreshToken = Extensions.ToSHA256(refreshToken),
				IsRevoked = false
			});

			await appDbContext.SaveChangesAsync();
		}

		public async Task ExtendSessionAsync(Guid sessionId, string refreshToken, DateTime expiresAt)
		{
			var session = await appDbContext.Sessions.FirstOrDefaultAsync(o => o.Id == sessionId);
			if (session != null)
			{
				session.ExpiresAt = expiresAt;
				session.LastRefreshToken = Extensions.ToSHA256(refreshToken);
				await appDbContext.SaveChangesAsync();
			}
		}

		public async Task InvalidateSessionAsync(Guid sessionId)
		{
			var session = await appDbContext.Sessions.FirstOrDefaultAsync(o => o.Id == sessionId);
			if (session != null)
			{
				session.IsRevoked = true;
				await appDbContext.SaveChangesAsync();
			}
		}

		public async Task<bool> IsSessionValidAsync(Guid sessionId, string refreshToken)
		{
			var session = await appDbContext.Sessions.FirstOrDefaultAsync(o => o.Id == sessionId);
			if (session == null || session.IsRevoked || session.ExpiresAt < DateTime.UtcNow)
			{
				return false;
			}
			var hashedToken = Extensions.ToSHA256(refreshToken);
			return session.LastRefreshToken == hashedToken;
		}
	}
}
