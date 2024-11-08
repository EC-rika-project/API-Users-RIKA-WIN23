namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class ResetPasswordDto
{
    public string JWT { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}

