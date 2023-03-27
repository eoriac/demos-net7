namespace Demo.API.Models
{
    public class UsersDto
    {
        // To be defined if needed
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
