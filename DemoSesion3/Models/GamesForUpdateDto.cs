using System.ComponentModel.DataAnnotations;

namespace DemoSesion3.Models
{
    public class GamesForUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
