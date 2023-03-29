using System.ComponentModel.DataAnnotations;

namespace Demos.API.Models.GamesDtos
{
    public class GamesForCreationDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
