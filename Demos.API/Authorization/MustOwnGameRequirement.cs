using Microsoft.AspNetCore.Authorization;

namespace Demo.API.Authorization
{
    public class MustOwnGameRequirement : IAuthorizationRequirement
    {
        public MustOwnGameRequirement()
        {
            
        }
    }
}
