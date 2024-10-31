using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class ShoppingCartItemFactory
{
    public static ShoppingCartItemDto Create(ShoppingCartItemEntity entity)
    {
        return new ShoppingCartItemDto
        {
            UserShoppingCartId = entity.UserShoppingCartId,
            ProductId = entity.ProductId,
            ProductPrice = entity.ProductPrice,
            Quantity = entity.Quantity,            
        };
    }

    public static ShoppingCartItemEntity Create(ShoppingCartItemDto dto)
    {
        return new ShoppingCartItemEntity
        {
            UserShoppingCartId = dto.UserShoppingCartId,
            ProductId = dto.ProductId,
            ProductPrice = dto.ProductPrice,
            Quantity = dto.Quantity,
        };
    }

    public static ICollection<ShoppingCartItemDto> Create(ICollection<ShoppingCartItemEntity> entities)
    {
        var dtos = new List<ShoppingCartItemDto>();
        foreach (var entity in entities)
        {
            dtos.Add(Create(entity));
        }

        return dtos;
    }

    public static ICollection<ShoppingCartItemEntity> Create(ICollection<ShoppingCartItemDto> dto)
    {
        var entities = new List<ShoppingCartItemEntity>();
        foreach (var entity in dto)
        {
            entities.Add(Create(entity));
        }

        return entities;
    }
}
