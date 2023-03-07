using DemoSesion3.Models;

namespace DemoSesion3
{
    public class UsersDataStore
    {
        public UsersDataStore()
        {
            Users = new List<UserDto>()
            {
                new UserDto()
                {
                    Id = 1,
                    Name = "Gandalf",
                    Email = "gandalf@mage.com"
                },
                new UserDto()
                {
                    Id = 2,
                    Name = "Frodo",
                    Email = "frodo@theshire.com"
                },
                new UserDto()
                {
                    Id = 3,
                    Name = "Trancos",
                    Email = "king@middleearth.com"
                }
            };
        }

        public List<UserDto> Users { get; set; }
    }
}
