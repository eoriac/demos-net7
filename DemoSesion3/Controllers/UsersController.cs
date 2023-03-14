using AutoMapper;
using DemoSesion3.Contracts;
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
        public async Task<ActionResult<IEnumerable<UserDto>>> Users()
        {
            var usersFromDb = await userRepository.GetUsers();

            var usersForResult = mapper.Map<IEnumerable<UserDto>>(usersFromDb);

            return Ok(usersForResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> User(Guid id)
        {
            var userFromDb = await userRepository.GetUser(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            var userForResult = mapper.Map<UserDto>(userFromDb);

            return Ok(userForResult);
        }
    }
}
