using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class UserWishListDto
{
    [ForeignKey("UserId")]
    public string UserId { get; set; } = null!;
    public List<string> ProductIds { get; set; } = new List<string>();
}
