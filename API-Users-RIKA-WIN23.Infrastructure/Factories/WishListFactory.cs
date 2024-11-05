using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class WishListFactory
{
    public static UserWishListDto Create(UserWishListEntity entity)
    {
        if (entity == null)
        {
            return null!;
        }
        return new UserWishListDto
        {
            UserId = entity.UserId,
            ProductIds = entity.ProductIds,
        };
    }

    public static UserWishListEntity Create(UserWishListDto dto)
    {
        if (dto == null)
        {
            return null!;
        }
        return new UserWishListEntity
        {
            UserId = dto.UserId,
            ProductIds = dto.ProductIds,
        };
    }
}
