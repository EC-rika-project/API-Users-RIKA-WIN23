using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class WishListFactory
{
    public static UserWishListDto Create(UserWishListEntity entity)
    {
        return new UserWishListDto
        {
            UserID = entity.UserID,
            ProductIDs = entity.ProductIDs,
        };
    }

    public static UserWishListEntity Create(UserWishListDto dto)
    {
        return new UserWishListEntity
        {
            UserID = dto.UserID,
            ProductIDs = dto.ProductIDs,
        };
    }
}
