

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;


public class UserShoppingCartEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();​

    public string UserId { get; set; } = null!;​

    public IEnumerable<ShoppingCartItemEntity> Products { get; set; } = [];​

}
