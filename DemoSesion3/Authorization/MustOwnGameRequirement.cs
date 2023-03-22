using Microsoft.AspNetCore.Authorization;

namespace DemoSesion3.Authorization
{
    public class MustOwnGameRequirement : IAuthorizationRequirement
    {
        public MustOwnGameRequirement()
        {
            
        }
    }
}
