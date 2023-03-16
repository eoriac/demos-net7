using AutoMapper;
using DemoSesion3.Contracts;
using DemoSesion3.Entities;
using DemoSesion3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoSesion3.Controllers
{
    [Route("api/users")]
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
            var usersFromDb = await userRepository.GetUsers();

            var usersForResult = mapper.Map<IEnumerable<UserDto>>(usersFromDb);

            return Ok(usersForResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var userFromDb = await userRepository.GetUser(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            var gamesUserFromDb = await userRepository.GetUserGames(userFromDb.Id);

            var userForResult = mapper.Map<UserDto>(userFromDb);
            userForResult.Games = mapper.Map<ICollection<GameDto>>(gamesUserFromDb);

            return Ok(userForResult);
        }
    }
}
