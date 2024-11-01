using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class AddressFactory
{
    public static UserAddressDto Create(UserAddressEntity entity)
    {
        if (entity == null)
        {
            return null!;
        }
        return new UserAddressDto
        {
            UserId = entity.UserId,
            AddressLine = entity.AddressLine,
            ApartmentNumber = entity.ApartmentNumber,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Country = entity.Country
        };
    }
    public static UserAddressEntity Create(UserAddressDto dto)
    {
        return new UserAddressEntity
        {
            UserId = dto.UserId,           
            AddressLine = dto.AddressLine,
            ApartmentNumber = dto.ApartmentNumber,
            PostalCode = dto.PostalCode,
            City = dto.City,
            Country = dto.Country
        };
    }

}
