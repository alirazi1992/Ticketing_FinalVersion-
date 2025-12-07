using Ticketing.Backend.Domain.Enums;

namespace Ticketing.Backend.Application.DTOs;

public record RegisterRequest(string FullName, string Email, string Password, UserRole Role);
public record LoginRequest(string Email, string Password);

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto? User { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
