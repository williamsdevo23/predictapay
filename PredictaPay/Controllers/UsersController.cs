using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PredictaPay.Domain.Entities;
using PredictaPay.DTOs.Users;
using PredictaPay.Infrastructure.Data;

namespace PredictaPay.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<AppUser> _passwordHasher = new();

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .AnyAsync(user => user.Email == request.Email, cancellationToken);

        if (existingUser)
        {
            return Conflict($"A user with email '{request.Email}' already exists.");
        }

        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, ToResponse(user));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .OrderBy(user => user.UserId)
            .Select(user => ToResponse(user))
            .ToListAsync(cancellationToken);

        return Ok(users);
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserResponseDto>> GetUserById(int userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.UserId == userId, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(user));
    }

    private static UserResponseDto ToResponse(AppUser user)
    {
        return new UserResponseDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }
}