using System.ComponentModel.DataAnnotations.Schema;

namespace DemoSesion3.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }
    }
}
