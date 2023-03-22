using Microsoft.AspNetCore.Authorization;

namespace Demos.Authorization
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy CanAddGame()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("company", "Steam")
                .RequireRole("GoldUser")
                .Build();
        }
    }
}