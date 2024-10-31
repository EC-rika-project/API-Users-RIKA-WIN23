using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class ShoppingCartFactory
{
    public static UserShoppingCartDto Create(UserShoppingCartEntity entity)
    {
        return new UserShoppingCartDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Products = ShoppingCartItemFactory.Create(entity.Products)
        };
    }

    public static UserShoppingCartEntity Create(UserShoppingCartDto dto)
    {
        return new UserShoppingCartEntity
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Products = ShoppingCartItemFactory.Create(dto.Products)
        };
    }

    public static ICollection<UserShoppingCartDto> Create(ICollection<UserShoppingCartEntity> entities)
    {
        var dtos = new List<UserShoppingCartDto>();
        foreach (var entity in entities)
        {
            dtos.Add(Create(entity));
        }

        return dtos;
    }

    public static ICollection<UserShoppingCartEntity> Create(ICollection<UserShoppingCartDto> dtos)
    {
        var entities = new List<UserShoppingCartEntity>();
        foreach (var dto in dtos)
        {
            entities.Add(Create(dto));
        }

        return entities;
    }
}
