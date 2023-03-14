using FluentMigrator;

namespace DemoSesion3.Migrations
{
    [Migration(2023130300002)]
    public class InitialSeed_2023130300002 : Migration
    {
        public override void Down()
        {
            Delete.FromTable("Users")
                .Row(new Entities.User
                {
                    Id = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                    Name = "Gandalf",
                    Email = "gandalf@mage.com"
                })
                .Row(new Entities.User
                {
                    Id = new Guid("70dc14ae-78cb-41fc-a675-02a7991660ff"),
                    Name = "Frodo",
                    Email = "frodo@theshire.com",
                })
                .Row(new Entities.User
                {
                    Id = new Guid("ba434a19-4e5d-49d6-98e0-43ff2b76482d"),
                    Name = "Trancos",
                    Email = "king@middleearth.com",
                });
        }

        public override void Up()
        {
            Insert.IntoTable("Users")
                .Row(new Entities.User
                {
                    Id = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                    Name = "Gandalf",
                    Email = "gandalf@mage.com",                    
                })
                .Row(new Entities.User
                {
                    Id = new Guid("70dc14ae-78cb-41fc-a675-02a7991660ff"),
                    Name = "Frodo",
                    Email = "frodo@theshire.com",
                })
                .Row(new Entities.User
                {
                    Id = new Guid("ba434a19-4e5d-49d6-98e0-43ff2b76482d"),
                    Name = "Trancos",
                    Email = "king@middleearth.com",
                });

            Insert.IntoTable("Games")
                .Row(new Entities.Game
                {
                    Id = new Guid("ff5d1798-7700-4670-8430-b9e2d4a8a26e"),
                    Name = "CoD",
                    Description = string.Empty,
                    UserId = new Guid("67fbac34-1ee1-4697-b916-1748861dd275")
                })
                .Row(new Entities.Game
                {
                    Id = new Guid("ebe02f86-93e8-4646-ac18-372269aba660"),
                    Name = "Tetris",
                    Description = string.Empty,
                    UserId = new Guid("70dc14ae-78cb-41fc-a675-02a7991660ff")
                })
                .Row(new Entities.Game
                {
                    Id = new Guid("578a323d-1168-4909-94af-df42d46cd39a"),
                    Name = "Mario Bros",
                    Description = string.Empty,
                    UserId = new Guid("70dc14ae-78cb-41fc-a675-02a7991660ff")
                });
        }
    }
}
