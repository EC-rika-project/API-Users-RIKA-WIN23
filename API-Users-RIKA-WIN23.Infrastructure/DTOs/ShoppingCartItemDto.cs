namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class ShoppingCartItemDto
{
    public string UserShoppingCartId { get; set; } = null!;
    public UserShoppingCartDto ShoppingCart { get; set; } = new();
    public string ProductId { get; set; } = null!;
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}