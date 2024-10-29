

//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;

//namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

//public class UserProfileDto
//{
//    [Required]
//    public string UserId { get; set; } = null!;​

//    [Required]
//    [DataType(DataType.EmailAddress)]
//    [RegularExpression(@"^\w+([-+.']\w+)@\w+([-.]\w+).\w{2,}$", ErrorMessage = "Email Or Password Invalid")]
//    public string Email { get; set; } = null!;​

//    [ProtectedPersonalData]
//    public string? FirstName { get; set; } ​

//    [ProtectedPersonalData]
//    public string? LastName { get; set; } ​

//    [ProtectedPersonalData]
//    public string ProfileImageUrl { get; set; } = "defaultUrl()";​

//    [ProtectedPersonalData]
//    public string? Gender { get; set; } ​

//    [ProtectedPersonalData]
//    public int Age { get; set; }​
//}
