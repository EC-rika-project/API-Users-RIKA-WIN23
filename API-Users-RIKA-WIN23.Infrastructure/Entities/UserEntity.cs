
using Microsoft.AspNetCore.Identity;

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;


public class UserEntity : IdentityUser
{
    public bool IsExternalAccount { get; set; } = false;​

    public UserProfileEntity? Profile { get; set; } = new();​

    public UserAddressEntity? Address { get; set; } = new();​

    public ICollection<UserShoppingCartEntity> ShoppingCarts { get; set; } = [];    

}
