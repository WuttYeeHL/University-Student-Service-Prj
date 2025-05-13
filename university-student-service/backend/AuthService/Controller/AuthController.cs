using AuthService.Data;
using AuthService.Dto;
using AuthService.Model;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokenService;

    private readonly PasswordService _passwordService;

    public AuthController(AppDbContext db, TokenService tokenService, PasswordService passwordService)
    {
        _db = db;
        _tokenService = tokenService;
        _passwordService = passwordService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model, [FromQuery] bool includeToken = false)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.LoginCode == model.LoginCode);

        if (user == null || !VerifyPassword(user, model.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = _tokenService.CreateToken(user);
        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        });

        if (includeToken)
            return Ok(new { token });

        return Ok();
    }

    [Authorize]
    [HttpGet("currentuser")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var loginCode = User.FindFirst("LoginCode")?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new { userId, loginCode, username, email });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return Ok(new { message = "Logged out" });
    }

    // It doesn't need in production
    [HttpGet("getHashedPassword")]
    public IActionResult GetHashedPassword([FromQuery] int id, [FromQuery] string username, [FromQuery] string email, [FromQuery] string loginCode, [FromQuery] string password)
    {
        var user = new User
        {
            Id = id,
            Username = username,
            Email = email,
            LoginCode = loginCode,
        };

        string hashed = _passwordService.HashPassword(user, password);

        return Ok(new { HashedPassword = hashed });
    }

    private bool VerifyPassword(User user, string password, string hash)
    {
        bool isValid = _passwordService.VerifyPassword(user, hash, password);
        return isValid;
    }
}
