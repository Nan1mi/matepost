using MatePost.DTOs;
using MatePost.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatePost.Controllers;

[ApiController]
[Route("api/[controller]")] // Шлях буде: /api/parcels
[Authorize] // Захищає всі методи у цьому файлі. Потрібен JWT токен!
public class ParcelsController : ControllerBase
{
    private readonly IParcelService _parcelService;

    public ParcelsController(IParcelService parcelService)
    {
        _parcelService = parcelService;
    }

    // Допоміжний метод, щоб дістати ID користувача з токена
    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet("cities")]
    [AllowAnonymous] // Відкрито для всіх (навіть без токена)
    public async Task<IActionResult> GetCities()
    {
        return Ok(await _parcelService.GetCitiesAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateParcelDto dto)
    {
        try
        {
            var userId = GetUserId(); // Дізнаємось, хто створює посилку
            var result = await _parcelService.CreateAsync(dto, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyParcels()
    {
        var userId = GetUserId();
        return Ok(await _parcelService.GetByUserAsync(userId));
    }

    [HttpGet("map")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMapParcels()
    {
        return Ok(await _parcelService.GetMapParcelsAsync());
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            return Ok(await _parcelService.GetByIdAsync(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("track/{trackingNumber}")]
    [AllowAnonymous]
    public async Task<IActionResult> Track(string trackingNumber)
    {
        try
        {
            return Ok(await _parcelService.GetByTrackingNumberAsync(trackingNumber));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateParcelStatusDto dto)
    {
        try
        {
            return Ok(await _parcelService.UpdateStatusAsync(id, dto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}