using System.ComponentModel.DataAnnotations;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class UserShoppingCartEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = null!;
    public ICollection<ShoppingCartItemEntity> Products { get; set; } = [];
}