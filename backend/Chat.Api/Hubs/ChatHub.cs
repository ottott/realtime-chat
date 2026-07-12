using Chat.Api.Data;
using Chat.Api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Chat.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ChatDbContext _db;


    public ChatHub(ChatDbContext db)
    {
        _db = db;
    }

    public async Task SendMessage(string message)
    {
        var username = Context.User?.Identity?.Name;

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new HubException("User not authenticated.");
        }

        var chatMessage = new Message
        {
            UserId = int.Parse(
                Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value
            ),
            Content = message
        };


        _db.Messages.Add(chatMessage);
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync(
            "ReceiveMessage",
            username,
            chatMessage.Content
        );
    }
}