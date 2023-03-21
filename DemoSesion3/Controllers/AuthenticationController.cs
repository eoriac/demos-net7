using DemoSesion3.Contracts;
using DemoSesion3.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoSesion3.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public class AuthenticationRequestBody
        {
            public string? UserId { get; set; }
            public string? Password { get; set; }
        }

        public AuthenticationController(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            this.userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            this.configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {
            var user = await ValidateUserCredentialsAsync(
                authenticationRequestBody.UserId,
                authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // Token generation
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("email", user.Email)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                configuration["Authentication:Issuer"],
                configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private async Task<User> ValidateUserCredentialsAsync(string? userId, string? password)
        {
            //var user = await userRepository.GetUserAsync(Guid.Parse(userId), password);

            return new User()
            {
                Id = Guid.NewGuid(),
                Name = userId ?? "",
                Email = "email@email.com"
            };
        }
    }
}
