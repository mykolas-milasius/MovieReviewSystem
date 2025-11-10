using Microsoft.AspNetCore.Identity;

namespace WebAPI.Entities
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        //public string Email { get; set; } = string.Empty;
        //public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Movie>? Movies { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Actor>? Actors { get; set; }
        public ICollection<Genre>? Genres { get; set; }
    }
}
