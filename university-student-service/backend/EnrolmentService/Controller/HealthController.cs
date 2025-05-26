using Microsoft.AspNetCore.Mvc;

namespace EnrolmentService.Controller
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Healthy");
    }
}
