using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnrolmentService.Services;
using System.Security.Claims;

namespace EnrolmentService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrolmentController : ControllerBase
{
    private readonly IStudentEnrolmentService _enrolmentService;

    public EnrolmentController(IStudentEnrolmentService enrolmentService)
    {
        _enrolmentService = enrolmentService;
    }

    [HttpGet("getEnrolmentByUserId")]
    public async Task<IActionResult> GetEnrolments([FromQuery] int userId)
    {
        var enrolments = await _enrolmentService.GetEnrolmentsByUserIdAsync(userId);
        return Ok(enrolments);
    }

    [HttpGet]
    public async Task<IActionResult> GetEnrolments()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var loginCode = User.FindFirst("LoginCode")?.Value;

        if (userId != null && loginCode != null)
        {
            var enrolments = await _enrolmentService.GetEnrolmentsByUserIdAsync(int.Parse(userId));
            return Ok(enrolments);
        }

        return Unauthorized();
    }
}
