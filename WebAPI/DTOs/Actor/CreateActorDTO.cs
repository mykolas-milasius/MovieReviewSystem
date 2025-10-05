namespace WebAPI.DTOs.Actor
{
    public class CreateActorDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Bio { get; set; } = string.Empty;
    }
}
