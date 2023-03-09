using System.ComponentModel.DataAnnotations;

namespace DemoSesion3.Models
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
