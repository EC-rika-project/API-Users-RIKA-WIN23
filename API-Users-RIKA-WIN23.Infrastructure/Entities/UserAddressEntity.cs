using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class UserAddressEntity
{
    [Required]
    [Key]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    public UserEntity? User { get; set; }

    [ProtectedPersonalData]
    public string? AddressLine { get; set; }

    [ProtectedPersonalData]
    public string? ApartmentNumber { get; set; }

    [RegularExpression(@"^\d{5}$", ErrorMessage = "PostalCode invalid, submit 5 digits only.")]
    public int PostalCode { get; set; } = 00000;
       
    public string? City { get; set; }

    public string? Country { get; set; }
}