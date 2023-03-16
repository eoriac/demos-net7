using DemoSesion3.Entities;

namespace DemoSesion3.Contracts
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetUserGamesAsync(Guid userId);

        public Task<Game> GetUserGameAsync(Guid userId, Guid gameId);

        public Task<int> DeleteUserGameAsync(Guid userId, Guid gameId);

        public Task<int> UpdateUserGameAsync(Guid gameId, Game gameForUpdate);

        public Task<Game> CreateUserGameAsync(Guid userId, Game game);

    }
}
