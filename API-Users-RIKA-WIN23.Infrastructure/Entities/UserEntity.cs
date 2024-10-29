using Microsoft.AspNetCore.Identity;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;

public class UserEntity : IdentityUser
{
    public bool IsExternalAccount { get; set; } = false;

    public UserProfileEntity? Profile { get; set; }

    public UserAddressEntity? Address { get; set; }

    public UserWishListEntity? WishList { get; set; }

    public ICollection<UserShoppingCartEntity>? ShoppingCarts { get; set; }
}