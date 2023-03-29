using Demo.API.Entities;

namespace Demo.API.Tests
{
    public class UsersDataStore
    {
        public static List<User> Users = new()
        {
            new User()
            {
                Id = new Guid("67fbac34-1ee1-4697-b916-1748861dd275"),
                Name = "Gandalf",
                Email = "gandalf@mage.com"
            },
            new User(){
                Id = new Guid("70dc14ae-78cb-41fc-a675-02a7991660ff"),
                Name = "Frodo",
                Email = "frodo@theshire.com",
            },
            new User()
            {
                Id = new Guid("ba434a19-4e5d-49d6-98e0-43ff2b76482d"),
                Name = "Trancos",
                Email = "king@middleearth.com",
            }
        };
    }
}
