using DemoSesion3.Entities;

namespace DemoSesion3.Contracts
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetUserGamesAsync(Guid userId);

        Task<Game> GetUserGameAsync(Guid userId, Guid gameId);

        Task<int> DeleteUserGameAsync(Guid userId, Guid gameId);

        Task<int> UpdateUserGameAsync(Guid gameId, Game gameForUpdate);

        Task<Game> CreateUserGameAsync(Guid userId, Game game);

        Task<bool> IsGameOwnerAsync(Guid userId, Guid gameId);
    }
}
