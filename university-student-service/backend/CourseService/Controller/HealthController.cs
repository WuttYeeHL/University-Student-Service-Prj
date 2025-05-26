using Microsoft.AspNetCore.Mvc;

namespace CourseService.Controller
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Healthy");
    }
}
