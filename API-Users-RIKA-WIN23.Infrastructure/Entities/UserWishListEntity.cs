using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class UserWishListEntity
{
    [Key]
    [ForeignKey("UserId")]
    public string UserID { get; set; } = null!;

    public UserEntity User { get; set; } = new();

    public List<string> ProductIDs { get; set; } = new List<string>();
}
