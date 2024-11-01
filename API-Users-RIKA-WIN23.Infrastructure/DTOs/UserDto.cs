using Microsoft.AspNetCore.Identity;

namespace API_Users_RIKA_WIN23.Infrastructure.DTOs;

public class UserDto
{
    public string Id { get; set; } = null!;
    public bool IsExternalAccount { get; set; } = false;
    public UserProfileDto? Profile { get; set; }
    public UserAddressDto? Address { get; set; }
    public UserWishListDto? WishList { get; set; }
    public ICollection<UserShoppingCartDto>? ShoppingCarts { get; set; }
}