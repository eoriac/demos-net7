using DemoSesion3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DemoSesion3.Controllers
{
    [Route("api/users/{userId}/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly UsersDataStore dataStore;

        public GamesController(UsersDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        [HttpGet(Name = "UsersGames")]
        public ActionResult<ICollection<GameDto>> UserGames(int userId)
        {
            var user = this.dataStore.Users.FirstOrDefault(usr => usr.Id == userId);

            if (user == null) 
            {
                return NotFound();
            }

            return Ok(user.Games);
        }

        [HttpGet("{gameId}", Name = "GetName")]
        public ActionResult<GameDto> UserGame(int userId, int gameId)
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
        public ActionResult<GameDto> CreateGame(int userId, GamesForCreationDto game)
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
                Id = ++maxGameId,
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
        public ActionResult<GameDto> UpdateGame(int userId, int gameId, GamesForUpdateDto game)
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
        public ActionResult PartialUpdate(int userId, int gameId, JsonPatchDocument<GamesForUpdateDto> patchDocument)
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

            patchDocument.ApplyTo(resultGameToPatch);

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
    }
}
