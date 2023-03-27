using System.ComponentModel.DataAnnotations;

namespace Demo.API.Models
{
    public class GamesForUpdateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
