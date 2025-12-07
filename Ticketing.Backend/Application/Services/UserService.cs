using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.Backend.Application.DTOs;
using Ticketing.Backend.Domain.Entities;
using Ticketing.Backend.Domain.Enums;
using Ticketing.Backend.Infrastructure.Auth;
using Ticketing.Backend.Infrastructure.Data;

namespace Ticketing.Backend.Application.Services;

public interface IUserService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request, UserRole creatorRole);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<IEnumerable<UserDto>> GetTechniciansAsync();
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request, UserRole creatorRole)
    {
        // Only allow admins to create elevated users
        if (request.Role != UserRole.Client && creatorRole != UserRole.Admin)
        {
            return null;
        }

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return null;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email.ToLowerInvariant(),
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        // Hash and store password just like ASP.NET Identity would
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = _jwtTokenGenerator.GenerateToken(user),
            User = MapToDto(user)
        };
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant());
        if (user == null)
        {
            return null;
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new AuthResponse
        {
            Token = _jwtTokenGenerator.GenerateToken(user),
            User = MapToDto(user)
        };
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? null : MapToDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .OrderBy(u => u.FullName)
            .Select(u => MapToDto(u))
            .ToListAsync();
    }

    public async Task<IEnumerable<UserDto>> GetTechniciansAsync()
    {
        return await _context.Users
            .Where(u => u.Role == UserRole.Technician)
            .OrderBy(u => u.FullName)
            .Select(u => MapToDto(u))
            .ToListAsync();
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role
    };
}
