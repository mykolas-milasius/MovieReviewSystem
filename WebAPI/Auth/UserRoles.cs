using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebAPI.Auth
{
    public class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);

        public static readonly IReadOnlyCollection<string> AllRoles = new [] { Admin, User };
    }
}
