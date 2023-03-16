﻿using Dapper;
using DemoSesion3.Context;
using DemoSesion3.Contracts;
using DemoSesion3.Entities;
using DemoSesion3.Models;
using System.Data;

namespace DemoSesion3.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly DapperContext context;

        public GameRepository(DapperContext context)
        {
            this.context = context;
        }

        public async Task<Game> CreateUserGame(Guid userId, Game game)
        {
            var query = "INSERT INTO Games (Id, Name, Description, UserId) VALUES (@Id, @Name, @Description, @UserId)";

            var gameId = Guid.NewGuid();

            var parameters = new DynamicParameters();
            parameters.Add("Id", gameId, DbType.Guid);
            parameters.Add("Name", game.Name, DbType.String);
            parameters.Add("Description", game.Description, DbType.String);
            parameters.Add("UserId", userId, DbType.Guid);

            using (var connection = context.CreateConnection())
            {
                var id = await connection.ExecuteAsync(query, parameters);

                var createdGame = new Game
                {
                    Id = gameId,
                    Name = game.Name,
                    Description = game.Description,
                };

                return createdGame;
            }
        }

        public Task<int> DeleteUserGame(Guid userId, Guid gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<Game> GetUserGame(Guid userId, Guid gameId)
        {
            var query = "SELECT * FROM Games WHERE UserId = @userId AND Id = @gameId";

            using (var connection = context.CreateConnection())
            {
                var game = await connection.QuerySingleOrDefaultAsync<Game>(query, new { userId, gameId });

                return game;
            }
        }

        public async Task<IEnumerable<Game>> GetUserGames(Guid userId)
        {
            var query = "SELECT * FROM Games WHERE UserId = @userId";

            using (var connection = context.CreateConnection())
            {
                var games = (await connection.QueryAsync<Game>(query, new { userId })).ToList();

                return games;
            }
        }

        public async Task<int> UpdateUserGame(Guid gameId, Game gameForUpdate)
        {
            var query = "UPDATE Games SET Name = @Name, Description = @Description WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", gameId, DbType.Guid);
            parameters.Add("Name", gameForUpdate.Name, DbType.String);
            parameters.Add("Description", gameForUpdate.Description, DbType.String);

            using (var connection = context.CreateConnection())
            {
                var rows = await connection.ExecuteAsync(query, parameters);

                return rows;
            }
        }
    }
}
