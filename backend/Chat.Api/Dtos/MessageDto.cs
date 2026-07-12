namespace Chat.Api.Dtos;

public class MessageDto
{
    public string Username { get; set; } = "";

    public string Content { get; set; } = "";

    public DateTime SentAt { get; set; }
}