namespace WebAPI.Helpers
{
	public class Extensions
	{
		public static string ToSHA256(string input)
		{
			using (var sha256 = System.Security.Cryptography.SHA256.Create())
			{
				var bytes = System.Text.Encoding.UTF8.GetBytes(input);
				var hash = sha256.ComputeHash(bytes);
				return Convert.ToBase64String(hash);
			}
		}
	}
}
