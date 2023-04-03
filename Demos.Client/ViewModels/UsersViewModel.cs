using Demos.Client.Models;

namespace Demos.Client.ViewModels
{
    public class UsersViewModel
    {
        public UsersViewModel(IEnumerable<UserModel> users)
        {
            Users = users;
        }

        public IEnumerable<UserModel> Users { get; private set; } = new List<UserModel>();
    }
}
