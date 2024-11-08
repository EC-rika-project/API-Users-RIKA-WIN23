﻿using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class UserFactory
{
    public static UserEntity Create(UserDto dto)
    {
        return new UserEntity
        {
            Id = dto.Id,
            IsExternalAccount = dto.IsExternalAccount,
            Profile = UserProfileFactory.Create(dto.Profile!),
            Address = AddressFactory.Create(dto.Address!),
            WishList = WishListFactory.Create(dto.WishList!),
            ShoppingCarts = ShoppingCartFactory.Create(dto.ShoppingCarts!),
        };
    }

    public static UserDto Create(UserEntity entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            UserName = entity.UserName,
            IsExternalAccount = entity.IsExternalAccount,
            Profile = UserProfileFactory.Create(entity.Profile!),
            Address = AddressFactory.Create(entity.Address!),
            WishList = WishListFactory.Create(entity.WishList!),
            ShoppingCarts = ShoppingCartFactory.Create(entity.ShoppingCarts!),
        };
    }

    public static IEnumerable<UserDto> Create(List<UserEntity> entities)
    {
        var dtos = new List<UserDto>();

        foreach(var entity in entities)
        {
            dtos.Add(Create(entity));
        }

        return dtos;
    }
}
