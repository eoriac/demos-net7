using Microsoft.AspNetCore.Mvc;

namespace DemoSesion3.Controllers
{
    [ApiController]
    [Route("api/heartbeat")]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        public string Beating()
        {
            return DateTime.Now.ToString();
        }
    }
}
