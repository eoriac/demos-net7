using AutoMapper;
using Demo.API.Contracts;
using Demo.API.Entities;
using Demo.API.Filters;
using Demo.API.Models;
using Demo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Demo.API.Controllers
{
    /// <summary>
    /// Games resource
    /// </summary>    
    [Authorize]
    //[ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [ApiController]
    //[Route("api/v{version:apiVersion}/users/{userId}/games")]
    [Route("api/users/{userId}/games")]
    public class GamesController : ControllerBase
    {
        const int maxGamesPageSize = 20;
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GamesController> logger;

        /// <summary>
        /// Ctor for GamesController
        /// </summary>
        /// <param name="gameRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GamesController(
            IGameRepository gameRepository,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<GamesController> logger)
        {
            this.gameRepository = gameRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// GET method to return all games for a given user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="name">Filter on user Name</param>
        /// <param name="queryPattern"></param>
        /// <param name="orderBy">Order by results</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size, default = 5, max = 20</param>
        /// <returns></returns>
        [HttpGet(Name = "UsersGames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<ActionResult<ICollection<GameDto>>> UserGamesAsync(Guid userId, string? name, string? queryPattern, string? orderBy,
            int pageNumber = 1, int pageSize = 5)
        {
            this.logger.LogDebug("Entering get games");

            var user = await this.userRepository.GetUserAsync(userId);
            if (user == null)
            {
                this.logger.LogInformation($"User not found for {userId}");
                return NotFound();
            }

            var gamesFromDb = await this.gameRepository.GetUserGamesAsync(userId);

            var gamesForResult = mapper.Map<IEnumerable<GameDto>>(gamesFromDb);

            return Ok(gamesForResult);

            //if (pageSize > maxGamesPageSize)
            //{
            //    pageSize = maxGamesPageSize;
            //}

            //var user = this.dataStore.Users.FirstOrDefault(usr => usr.Id == userId);

            //if (user == null) 
            //{
            //    return NotFound();
            //}

            //var gamesResult = user.Games
            //    .Skip(pageSize * (pageNumber - 1))
            //    .Take(pageSize);

            //if (!string.IsNullOrEmpty(name))
            //{
            //    gamesResult = user.Games.Where(gm => gm.Name == name)
            //        .Skip(pageSize * (pageNumber - 1))
            //        .Take(pageSize);
            //}

            //if (!string.IsNullOrEmpty(queryPattern))
            //{
            //    gamesResult = user.Games.Where(gm => gm.Name.Contains(queryPattern) 
            //    || (!string.IsNullOrEmpty(gm.Description) && string.Compare(gm.Description, queryPattern, StringComparison.InvariantCultureIgnoreCase) != 0))
            //        .Skip(pageSize * (pageNumber - 1))
            //        .Take(pageSize);
            //}

            //var gameList = gamesResult.ToList();

            //if (!string.IsNullOrEmpty(orderBy))
            //{
            //    gameList = gamesResult.OrderBy(gm => orderBy).ToList();
            //}

            //var totalItems = gameList.Count;

            //var paginationMetadata = new PaginationMetadata(pageSize, pageNumber, totalItems);

            //Response.Headers.Add("X-Pagination",
            //    JsonSerializer.Serialize(paginationMetadata));

            //return Ok(gameList);
        }

        /// <summary>
        /// Get a User Game
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns>A <seealso cref="GameDto"/>GameDto object</returns>
        [HttpGet("{gameId}", Name = "GetGame")]
        // [SpecificHeader]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameDto>> UserGameAsync(Guid userId, Guid gameId)
        {
            var gamesFromDb = await this.gameRepository.GetUserGameAsync(userId, gameId);

            if (gamesFromDb == null)
            {
                return NotFound();
            }

            var gamesForResult = mapper.Map<IEnumerable<GameDto>>(gamesFromDb);

            return Ok(gamesForResult);
        }

        [HttpPost]
        //[Authorize(Roles = "GoldUser")]
        [Authorize(Policy = "UserCanAddGame")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameDto>> CreateGame(Guid userId, GamesForCreationDto game)
        {            
            var user = await this.userRepository.GetUserAsync(userId);
            
            if (user == null)
            {
                return NotFound();
            }

            //var userId = User.Claims
            //    .FirstOrDefault(c => c.Type == "sub")?.Value;
            //            if (userId == null)
            //            {
            //                throw new Exception("User identifier is missing from token.");
            //            }
            //entityGame.UserId = userId;

            var entityGame = mapper.Map<Game>(game);

            entityGame.Id = Guid.NewGuid();
            entityGame.UserId = userId;

            var createdGame = await this.gameRepository.CreateUserGameAsync(userId, entityGame);
            
            var resultGame = mapper.Map<GameDto>(createdGame);

            return CreatedAtRoute(
                "GetGame",
                new { 
                    userId = userId, 
                    gameId = resultGame.Id 
                },
                resultGame
            );
        }

        [HttpPut("{gameId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameDto>> UpdateGame(Guid userId, Guid gameId, GamesForUpdateDto game)
        {
            var user = await this.userRepository.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultGameFromDb = await this.gameRepository.GetUserGameAsync(userId, gameId);

            if (resultGameFromDb == null)
            {
                return NotFound();
            }

            resultGameFromDb = mapper.Map<Game>(game);

            var rows = await this.gameRepository.UpdateUserGameAsync(gameId, resultGameFromDb);

            if (rows == 0)
            {
                //
            }

            return NoContent();
        }

        [HttpPatch("{gameId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PartialUpdate(Guid userId, Guid gameId, JsonPatchDocument<GamesForUpdateDto> patchDocument)
        {
            var user = await this.userRepository.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultGameFromDb = await this.gameRepository.GetUserGameAsync(userId, gameId);

            if (resultGameFromDb == null)
            {
                return NotFound();
            }

            var resultGameToPatch = mapper.Map<GamesForUpdateDto>(resultGameFromDb);

            patchDocument.ApplyTo(resultGameToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(resultGameToPatch))
            {
                return BadRequest(ModelState);
            }

            resultGameFromDb = mapper.Map<Game>(resultGameToPatch);

            var rows  = await this.gameRepository.UpdateUserGameAsync(gameId, resultGameFromDb);

            return NoContent();
        }

        [HttpDelete("{gameId}")]
        [Authorize(Policy = "MustOwnGame")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(Guid userId, Guid gameId)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

    }
}
