namespace WebAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        //Navigational
        public ICollection<Movie>? Movies { get; set; }
    }
}
