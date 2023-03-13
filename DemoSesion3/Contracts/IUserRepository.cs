using DemoSesion3.Entities;

namespace DemoSesion3.Contracts
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();

        public Task<User> GetUser(Guid id);

        public Task DeleteUser(Guid id);
    }
}
