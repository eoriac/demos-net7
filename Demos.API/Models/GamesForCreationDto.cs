using System.ComponentModel.DataAnnotations;

namespace Demo.API.Models
{
    public class GamesForCreationDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; } = String.Empty;
    }
}
