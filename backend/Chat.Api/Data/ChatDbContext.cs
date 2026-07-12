using Microsoft.EntityFrameworkCore;
using Chat.Api.Models;

namespace Chat.Api.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Message> Messages { get; set; }

}