using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class UserProfileEntity
{
    [Required]
    [Key]
    [ForeignKey("UserId")]
    public string UserId { get; set; } = null!;

    public UserEntity? User { get; set; }

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
