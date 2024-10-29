

using System.ComponentModel.DataAnnotations;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;


public class UserAddressEntity
{
    [Required]
    public string UserId { get; set; } = null!​

    [Required]
    public string AddressLine { get; set; } = null!;​  

    public string? ApartmentNumber { get; set; }​

    [RegularExpression("^\\d{3}\\s\\d{2}$", ErrorMessage = "Please enter a valid postal code format (e.g. 123 45).")]
    public int PostalCode { get; set; }​ 

    [Required]
    public string City { get; set; } = null!;​

    [Required]
    public string Country { get; set; } = null!;​
}
