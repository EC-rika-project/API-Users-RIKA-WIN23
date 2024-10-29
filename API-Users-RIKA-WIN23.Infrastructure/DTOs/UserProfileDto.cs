using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class UserProfileDto
{
    [Required]
    [ForeignKey("UserId")]
    public string UserId { get; set; } = null!;

    public UserDto User { get; set; } = new();

    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^\w+([-+.']\w+)@\w+([-.]\w+).\w{2,}$", ErrorMessage = "Email Or Password Invalid")]
    public string Email { get; set; } = null!;

    [ProtectedPersonalData]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    public string? LastName { get; set; }

    [ProtectedPersonalData]
    public string ProfileImageUrl { get; set; } = "defaultUrl()";

    [ProtectedPersonalData]
    public string? Gender { get; set; }

    [ProtectedPersonalData]
    public int Age { get; set; }
}
