using Dapper;
using DemoSesion3.Context;
using DemoSesion3.Contracts;
using DemoSesion3.Entities;

namespace DemoSesion3.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var query = "SELECT Id, Name, Email, AvatarUrl FROM Users";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query);
                return users.ToList();
            }
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            
            var query = "SELECT * FROM Users WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var User = await connection.QuerySingleOrDefaultAsync<User>(query, new { id });

                return User;
            }

            /*
            var query = "SELECT * FROM Users WHERE Id = @Id;" +
                        "SELECT * FROM Games WHERE UserId = @Id";

            using (var connection = _context.CreateConnection())
            using (var multiQuery = await connection.QueryMultipleAsync(query, new { id }))
            {
                var user = await multiQuery.ReadSingleOrDefaultAsync<User>();
                if (user != null)
                {
                    user.Games = (await multiQuery.ReadAsync<Game>()).ToList();
                }

                return user;
            }
            */
        }

        public async Task<ICollection<Game>> GetUserGamesAsync(Guid userId)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id;" +
                        "SELECT * FROM Games WHERE UserId = @Id";

            using (var connection = _context.CreateConnection())
            using (var multiQuery = await connection.QueryMultipleAsync(query, new { Id = userId }))
            {
                var resultGames = new List<Game>();

                var user = await multiQuery.ReadSingleOrDefaultAsync<User>();
                if (user != null)
                {
                    resultGames = (await multiQuery.ReadAsync<Game>()).ToList();
                }

                return resultGames;
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var query = "DELETE FROM Users WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }
    }
}
