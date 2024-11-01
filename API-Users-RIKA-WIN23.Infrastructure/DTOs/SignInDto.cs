using System.ComponentModel.DataAnnotations;

namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class SignInDto
{
    [RegularExpression(@"^[^\s@]+@[^\s@]+.[^\s@]{2,}$", ErrorMessage = "Email Or Password Invalid")]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,}$", ErrorMessage = "Email Or Password Invalid")]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}