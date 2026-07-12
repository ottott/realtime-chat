using Chat.Api.Data;
using Chat.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.Endpoints;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        app.MapGet("/messages", async (ChatDbContext db) =>
        {
            var messages = await db.Messages
                .OrderByDescending(m => m.SentAt)
                .Take(50)
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageDto
                {
                    Username = m.Username,
                    Content = m.Content,
                    SentAt = m.SentAt
                })
                .ToListAsync();

            return Results.Ok(messages);
        });
    }
}