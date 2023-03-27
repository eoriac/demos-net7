using Demo.API.Entities;

namespace Demo.API.Contracts
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsersAsync();

        public Task<User> GetUserAsync(Guid id);

        public Task DeleteUserAsync(Guid id);

        public Task<ICollection<Game>> GetUserGamesAsync(Guid userId);
    }
}
