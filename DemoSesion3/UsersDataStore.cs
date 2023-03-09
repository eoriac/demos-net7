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
                    Email = "gandalf@mage.com",

                    Games = new List<GameDto>()
                    {
                        new GameDto()
                        {
                            Id = 1,
                            Name = "CoD",
                        },
                    }
                },
                new UserDto()
                {
                    Id = 2,
                    Name = "Frodo",
                    Email = "frodo@theshire.com",

                    Games = new List<GameDto>()
                    {
                        new GameDto()
                        {
                            Id = 3,
                            Name = "Tetris",
                        }
                    }
                },
                new UserDto()
                {
                    Id = 3,
                    Name = "Trancos",
                    Email = "king@middleearth.com",

                    Games = new List<GameDto>()
                    {
                        new GameDto()
                        {
                            Id = 1,
                            Name = "CoD",
                        },
                        new GameDto()
                        {
                            Id = 2,
                            Name = "Mario Bros.",
                        },
                        new GameDto()
                        {
                            Id = 3,
                            Name = "Tetris",
                        }
                    }
                }
            };
        }

        public List<UserDto> Users { get; set; }
    }
}
