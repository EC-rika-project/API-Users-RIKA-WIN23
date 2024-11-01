namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class SignUpDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}