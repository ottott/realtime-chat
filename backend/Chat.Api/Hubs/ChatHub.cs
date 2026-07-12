using Chat.Api.Data;
using Chat.Api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

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
            Username = username,
            Content = message
        };


        _db.Messages.Add(chatMessage);
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync(
            "ReceiveMessage",
            chatMessage.Username,
            chatMessage.Content
        );
    }
}