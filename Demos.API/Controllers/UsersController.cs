using AutoMapper;
using Demo.API.Contracts;
using Demo.API.Entities;
using Demo.API.Models;
using Demos.API.Models.GamesDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var usersFromDb = await userRepository.GetUsersAsync();

            var usersForResult = mapper.Map<IEnumerable<UserDto>>(usersFromDb);

            return Ok(usersForResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var userFromDb = await userRepository.GetUserAsync(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            var gamesUserFromDb = await userRepository.GetUserGamesAsync(userFromDb.Id);

            var userForResult = mapper.Map<UserDto>(userFromDb);
            userForResult.Games = mapper.Map<ICollection<GameDto>>(gamesUserFromDb);

            return Ok(userForResult);
        }
    }
}
