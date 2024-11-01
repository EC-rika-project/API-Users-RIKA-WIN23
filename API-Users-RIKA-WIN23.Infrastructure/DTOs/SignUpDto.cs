using System.ComponentModel.DataAnnotations;

namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class SignUpDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [RegularExpression(@"^[^\s@]+@[^\s@]+.[^\s@]{2,}$", ErrorMessage = "Email invalid.")]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,}$", ErrorMessage = "Password invalid.")]
    public string Password { get; set; } = null!;
}