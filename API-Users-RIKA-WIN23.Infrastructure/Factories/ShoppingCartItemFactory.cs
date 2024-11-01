using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class ShoppingCartItemFactory
{
    public static ShoppingCartItemDto Create(ShoppingCartItemEntity entity)
    {
        if (entity == null)
        {
            return null!;
        }
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
        if (dto == null)
        {
            return null!;
        }
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
        if (entities == null)
        {
            return null!;
        }
        var dtos = new List<ShoppingCartItemDto>();
        foreach (var entity in entities)
        {
            dtos.Add(Create(entity));
        }

        return dtos;
    }

    public static ICollection<ShoppingCartItemEntity> Create(ICollection<ShoppingCartItemDto> dtos)
    {
        if (dtos == null)
        {
            return null!;
        }
        var entities = new List<ShoppingCartItemEntity>();
        foreach (var dto in dtos)
        {
            entities.Add(Create(dto));
        }

        return entities;
    }
}
