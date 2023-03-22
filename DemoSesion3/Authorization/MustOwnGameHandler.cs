using DemoSesion3.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace DemoSesion3.Authorization
{
    public class MustOwnGameHandler : AuthorizationHandler<MustOwnGameRequirement>
    {
        private readonly IGameRepository gameRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MustOwnGameHandler(
            IGameRepository gameRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnGameRequirement requirement)
        {
            var gameId = httpContextAccessor.HttpContext.GetRouteValue("gameId")?.ToString();

            if (Guid.TryParse(gameId, out Guid gameIdAsGuid) == false)
            {
                context.Fail();
                return;
            }

            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userId == null)
            {
                context.Fail();
                return;
            }

            if (Guid.TryParse(userId, out Guid userIdAsGuid) == false)
            {
                context.Fail();
                return;
            }

            if (await this.gameRepository.IsGameOwnerAsync(userIdAsGuid, gameIdAsGuid) == false)
            {
                context.Fail();
                return;
            }
        }
    }
}
