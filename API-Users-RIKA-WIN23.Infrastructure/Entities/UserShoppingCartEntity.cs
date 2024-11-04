using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class UserShoppingCartEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public UserEntity? User { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;
    public ICollection<ShoppingCartItemEntity> Products { get; set; } = [];
}