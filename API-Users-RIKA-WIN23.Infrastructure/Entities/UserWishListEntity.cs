using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class UserWishListEntity
{
    [Required]
    [Key]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    public UserEntity? User { get; set; }

    public List<string> ProductIds { get; set; } = new List<string>();
}
