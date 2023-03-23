using Microsoft.AspNetCore.Mvc;

namespace DemoSesion3.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/heartbeat")]
    [Route("api/heartbeat")]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("2.0")]
        public string Beating()
        {
            return $"{DateTime.Now} from v2";
        }
    }
}
