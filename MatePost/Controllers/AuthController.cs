using MatePost.DTOs;
using MatePost.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatePost.Controllers;

[ApiController]
[Route("api/[controller]")] // Шлях буде: /api/auth
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Підключаємо наш сервіс
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")] // Шлях: /api/auth/register
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result); // Повертає статус 200 та згенерований токен
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message }); // Повертає помилку, якщо email зайнятий
        }
    }

    [HttpPost("login")] // Шлях: /api/auth/login
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}