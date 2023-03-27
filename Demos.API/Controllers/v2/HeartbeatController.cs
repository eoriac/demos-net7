using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers.v2
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
