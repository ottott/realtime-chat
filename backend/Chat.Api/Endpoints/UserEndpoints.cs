using Chat.Api.Data;
using Microsoft.EntityFrameworkCore;
using Chat.Api.Models;
using Chat.Api.Dtos;
using Chat.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Chat.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", async (ChatDbContext db) =>
        {
            return await db.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                })
                .ToListAsync();
        });



        app.MapPost("/users", async (CreateUserDto dto, ChatDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                return Results.BadRequest("Username is required.");
            }

            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute()
                .IsValid(dto.Email))
            {
                return Results.BadRequest("Invalid email address.");
            }

            if (dto.Password.Length < 8)
            {
                return Results.BadRequest("Password must be at least 8 characters.");
            }

            bool usernameExists = await db.Users
                .AnyAsync(user => user.Username == dto.Username);

            if (usernameExists)
            {
                return Results.Conflict("Username already exists.");
            }

            bool emailExists = await db.Users
                .AnyAsync(user => user.Email == dto.Email);

            if (emailExists)
            {
                return Results.Conflict("Email already exists.");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();

            return Results.Created(
                $"/users/{user.Id}",
                new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                }
            );
        });

        app.MapPost("/login", async (LoginDto dto, ChatDbContext db, JwtService jwtService) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return Results.Unauthorized();
            }

            bool passwordCorrect =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordCorrect)
            {
                return Results.Unauthorized();
            }
            
            string token = jwtService.GenerateToken(user);

            return Results.Ok(new
            {
                Token = token
            });

        });


        app.MapGet("/me", (ClaimsPrincipal user) =>
        {
            return Results.Ok(new
            {
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Username = user.Identity?.Name
            });
        })
        .RequireAuthorization();

    }
}