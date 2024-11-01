using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class UserProfileDto
{
    [Required]
    [ForeignKey("UserId")]
    public string UserId { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[^\s@]+@[^\s@]+.[^\s@]{2,}$", ErrorMessage = "Email invalid.")]
    public string Email { get; set; } = null!;

    [ProtectedPersonalData]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    public string? LastName { get; set; }

    [ProtectedPersonalData]
    public string? ProfileImageUrl { get; set; }

    [ProtectedPersonalData]
    public string? Gender { get; set; }

    [ProtectedPersonalData]
    public int Age { get; set; }
}
