using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class UserFactory
{
    public static UserEntity Create(UserDto dto)
    {
        return new UserEntity
        {
            IsExternalAccount = dto.IsExternalAccount,
            Profile = UserProfileFactory.Create(dto.Profile!) ?? null,
            Address = AddressFactory.Create(dto.Address!) ?? null,
            WishList = WishListFactory.Create(dto.WishList!) ?? null,
            ShoppingCarts = ShoppingCartFactory.Create(dto.ShoppingCarts!) ?? null,
        };
    }

    public static UserDto Create(UserEntity entity)
    {
        return new UserDto
        {
            IsExternalAccount = entity.IsExternalAccount,
            Profile = UserProfileFactory.Create(entity.Profile!) ?? null,
            Address = AddressFactory.Create(entity.Address!) ?? null,
            WishList = WishListFactory.Create(entity.WishList!) ?? null,
            ShoppingCarts = ShoppingCartFactory.Create(entity.ShoppingCarts!) ?? null,
        };
    }
}
