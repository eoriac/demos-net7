namespace Demo.API.Models
{
    /// <summary>
    /// Game resourcer DTO
    /// </summary>
    public class GameDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
