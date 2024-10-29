

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class ShoppingCartItemEntity
{
    public string UserShoppingCartId { get; set; } = null!;​

    public string ProductId { get; set; } = null!;​

    public decimal ProductPrice { get; set; }
    
    public int Quantity { get; set; }

    //eventuellt totalt pris
}
