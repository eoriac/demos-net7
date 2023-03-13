using DemoSesion3.Models;
using DemoSesion3.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DemoSesion3.Controllers
{
    [Route("api/users/{userId}/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly UsersDataStore dataStore;
        private readonly INotificationService notificationService;
        const int maxGamesPageSize = 20;

        public GamesController(UsersDataStore dataStore, INotificationService notificationService)
        {
            this.dataStore = dataStore;
            this.notificationService = notificationService;
        }

        [HttpGet(Name = "UsersGames")]
        public ActionResult<ICollection<GameDto>> UserGames(Guid userId, string? name, string? queryPattern, string? orderBy,
            int pageNumber = 1, int pageSize = 5)
        {
            if (pageSize > maxGamesPageSize)
            {
                pageSize = maxGamesPageSize;
            }

            var user = this.dataStore.Users.FirstOrDefault(usr => usr.Id == userId);

            if (user == null) 
            {
                return NotFound();
            }

            var gamesResult = user.Games
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            if (!string.IsNullOrEmpty(name))
            {
                gamesResult = user.Games.Where(gm => gm.Name == name)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize);
            }

            if (!string.IsNullOrEmpty(queryPattern))
            {
                gamesResult = user.Games.Where(gm => gm.Name.Contains(queryPattern) 
                || (!string.IsNullOrEmpty(gm.Description) && string.Compare(gm.Description, queryPattern, StringComparison.InvariantCultureIgnoreCase) != 0))
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize);
            }

            var gameList = gamesResult.ToList();

            if (!string.IsNullOrEmpty(orderBy))
            {
                gameList = gamesResult.OrderBy(gm => orderBy).ToList();
            }

            var totalItems = gameList.Count;

            var paginationMetadata = new PaginationMetadata(pageSize, pageNumber, totalItems);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(gameList);
        }

        [HttpGet("{gameId}", Name = "GetName")]
        public ActionResult<GameDto> UserGame(Guid userId, Guid gameId)
        {
            var user = this.dataStore.Users.FirstOrDefault(usr => usr.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var game = user.Games.FirstOrDefault(gm => gm.Id == gameId);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpPost]
        public ActionResult<GameDto> CreateGame(Guid userId, GamesForCreationDto game)
        {
            var user = this.dataStore.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            // demo purposes
            var maxGameId = this.dataStore.Users.SelectMany(
                             c => c.Games).Max(gm => gm.Id);

            var resultGame = new GameDto()
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Description = game.Description
            };

            user.Games.Add(resultGame);

            return CreatedAtRoute(
                "GetName",
                new { 
                    userId = userId, 
                    gameId = resultGame.Id 
                },
                resultGame
            );
        }

        [HttpPut("{gameId}")]
        public ActionResult<GameDto> UpdateGame(Guid userId, Guid gameId, GamesForUpdateDto game)
        {
            var user = this.dataStore.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultGame = user.Games.FirstOrDefault(gm => gm.Id == gameId);

            if (resultGame == null)
            {
                return NotFound();
            }

            resultGame.Name = game.Name;
            resultGame.Description = game.Description;

            return NoContent();
        }

        [HttpPatch("{gameId}")]
        public ActionResult PartialUpdate(Guid userId, Guid gameId, JsonPatchDocument<GamesForUpdateDto> patchDocument)
        {
            var user = this.dataStore.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultGameFromDb = user.Games.FirstOrDefault(gm => gm.Id == gameId);

            if (resultGameFromDb == null)
            {
                return NotFound();
            }

            var resultGameToPatch = new GamesForUpdateDto()
            {
                Name = resultGameFromDb.Name,
                Description = resultGameFromDb.Description
            };

            patchDocument.ApplyTo(resultGameToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(resultGameToPatch))
            {
                return BadRequest(ModelState);
            }

            resultGameFromDb.Name = resultGameToPatch.Name;
            resultGameFromDb.Description = resultGameToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{gameId}")]
        public ActionResult Delete(Guid userId, Guid gameId)
        {
            var user = this.dataStore.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultGame = user.Games.FirstOrDefault(gm => gm.Id == gameId);

            if (resultGame == null)
            {
                return NotFound();
            }

            user.Games.Remove(resultGame);

            notificationService.Send("Se ha eliminar0", "sdfsdfd");

            return NoContent();
        }

    }
}
