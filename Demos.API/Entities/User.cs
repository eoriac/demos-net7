using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.API.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }
    }
}
