using Microsoft.AspNetCore.Mvc;

namespace DemoSesion3.Controllers.v3
{
    [ApiController]
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/heartbeat")]
    [Route("api/heartbeat")]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("3.0")]
        public string Beating()
        {
            return $"{DateTime.Now} from v3 new";
        }
    }
}
