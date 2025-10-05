namespace WebAPI.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Bio { get; set; } = string.Empty;

        //Navigational
        public ICollection<Movie>? Movies { get; set; }
    }
}
