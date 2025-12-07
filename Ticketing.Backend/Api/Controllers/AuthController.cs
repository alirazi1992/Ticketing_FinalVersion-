using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Backend.Application.DTOs;
using Ticketing.Backend.Application.Services;
using Ticketing.Backend.Domain.Enums;

namespace Ticketing.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        // Anonymous callers are treated as clients; admins can elevate roles via the service check
        var role = User.Identity?.IsAuthenticated == true
            ? Enum.Parse<UserRole>(User.FindFirstValue(ClaimTypes.Role) ?? UserRole.Client.ToString())
            : UserRole.Client;
        var response = await _userService.RegisterAsync(request, role);
        if (response == null)
        {
            return Forbid();
        }
        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var response = await _userService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized();
        }
        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me()
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.Email);
        if (!Guid.TryParse(idValue, out var userId))
        {
            return Unauthorized();
        }
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
