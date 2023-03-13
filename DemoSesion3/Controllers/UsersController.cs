using DemoSesion3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoSesion3.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDataStore dataStore;

        public UsersController(UsersDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        [HttpGet]
        public ActionResult<ICollection<UserDto>> Users()
        {
            return Ok(dataStore.Users);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> User(Guid id)
        {
            var user = dataStore.Users.FirstOrDefault(user => user.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
