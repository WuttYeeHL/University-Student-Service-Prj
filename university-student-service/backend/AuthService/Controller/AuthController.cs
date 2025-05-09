using AuthService.Data;
using AuthService.Dto;
using AuthService.Model;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.LoginCode == model.LoginCode);

        if (user == null || !VerifyPassword(user, model.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = _tokenService.CreateToken(user);
        return Ok(new { token });
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
