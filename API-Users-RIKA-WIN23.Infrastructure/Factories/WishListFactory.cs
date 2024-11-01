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
            UserID = entity.UserID,
            ProductIDs = entity.ProductIDs,
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
            UserID = dto.UserID,
            ProductIDs = dto.ProductIDs,
        };
    }
}
