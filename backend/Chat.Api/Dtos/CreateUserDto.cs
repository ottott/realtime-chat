using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Dtos;

public class CreateUserDto
{
    [Required]
    public string Username { get; set; } = "";
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = "";
}