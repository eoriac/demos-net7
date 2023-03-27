namespace Demo.API.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? AvatarUrl { get; set; }

        public int NumberOfGames
        {
            get
            {
                return this.Games.Count;
            }
        }

        public ICollection<GameDto> Games { get; set; } = new List<GameDto>();
    }
}
