using Microsoft.AspNetCore.Mvc;

namespace DemoSesion3.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/heartbeat")]
    [Route("api/heartbeat")]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public string Beating()
        {
            return DateTime.Now.ToString();
        }
    }
}
