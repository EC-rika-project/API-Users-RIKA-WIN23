namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class UserShoppingCartDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = null!;
    public ICollection<ShoppingCartItemDto> Products { get; set; } = [];
}
