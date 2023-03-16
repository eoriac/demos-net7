using DemoSesion3.Entities;

namespace DemoSesion3.Contracts
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetUserGames(Guid userId);

        public Task<Game> GetUserGame(Guid userId, Guid gameId);

        public Task<int> DeleteUserGame(Guid userId, Guid gameId);

        public Task<int> UpdateUserGame(Guid gameId, Game gameForUpdate);

        public Task<Game> CreateUserGame(Guid userId, Game game);

    }
}
