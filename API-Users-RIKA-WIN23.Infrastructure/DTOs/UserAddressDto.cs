using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class UserAddressDto
{
    [Required]
    [ForeignKey("UserId")]
    public string UserId { get; set; } = null!;

    public UserDto User { get; set; } = new();

    [Required]
    public string AddressLine { get; set; } = null!;

    public string? ApartmentNumber { get; set; }

    [RegularExpression(@"^\d{5}$", ErrorMessage = "PostalCode invalid, please submit 5 digits only.")]
    public int PostalCode { get; set; }

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string Country { get; set; } = null!;
}