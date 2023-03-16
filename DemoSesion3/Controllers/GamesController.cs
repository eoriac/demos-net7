﻿using AutoMapper;
using DemoSesion3.Contracts;
using DemoSesion3.Entities;
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
        const int maxGamesPageSize = 20;
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GamesController(
            IGameRepository gameRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.gameRepository = gameRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet(Name = "UsersGames")]
        public async Task<ActionResult<ICollection<GameDto>>> UserGamesAsync(Guid userId, string? name, string? queryPattern, string? orderBy,
            int pageNumber = 1, int pageSize = 5)
        {
            var gamesFromDb = await this.gameRepository.GetUserGames(userId);

            if (gamesFromDb == null)
            {
                return NotFound();
            }

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

        [HttpGet("{gameId}", Name = "GetGame")]
        public async Task<ActionResult<GameDto>> UserGameAsync(Guid userId, Guid gameId)
        {
            var gamesFromDb = await this.gameRepository.GetUserGame(userId, gameId);

            if (gamesFromDb == null)
            {
                return NotFound();
            }

            var gamesForResult = mapper.Map<IEnumerable<GameDto>>(gamesFromDb);

            return Ok(gamesForResult);
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame(Guid userId, GamesForCreationDto game)
        {
            var user = await this.userRepository.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }

            var entityGame = mapper.Map<Game>(game);

            entityGame.Id = Guid.NewGuid();
            entityGame.UserId = userId;

            var createdGame = await this.gameRepository.CreateUserGame(userId, entityGame);
            
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
        public async Task<ActionResult<GameDto>> UpdateGame(Guid userId, Guid gameId, GamesForUpdateDto game)
        {
            var user = await this.userRepository.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultGameFromDb = await this.gameRepository.GetUserGame(userId, gameId);

            if (resultGameFromDb == null)
            {
                return NotFound();
            }

            resultGameFromDb = mapper.Map<Game>(game);

            var rows = await this.gameRepository.UpdateUserGame(gameId, resultGameFromDb);

            if (rows == 0)
            {
                //
            }

            return NoContent();
        }

        [HttpPatch("{gameId}")]
        public async Task<ActionResult> PartialUpdate(Guid userId, Guid gameId, JsonPatchDocument<GamesForUpdateDto> patchDocument)
        {
            var user = await this.userRepository.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }

            var resultGameFromDb = await this.gameRepository.GetUserGame(userId, gameId);

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

            var rows  = await this.gameRepository.UpdateUserGame(gameId, resultGameFromDb);

            return NoContent();
        }

        [HttpDelete("{gameId}")]
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
