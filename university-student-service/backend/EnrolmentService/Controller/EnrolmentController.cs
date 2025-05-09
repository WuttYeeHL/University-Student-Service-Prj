using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnrolmentService.Dto;
using EnrolmentService.Services;

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
    public async Task<IActionResult> GetEnrolments([FromBody] EnrolmentRequest request)
    {
        var enrolments = await _enrolmentService.GetEnrolmentsByUserIdAsync(request.UserId);
        return Ok(enrolments);
    }
}
