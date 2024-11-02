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
        if (dto == null)
        {
            return null!;
        }
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

    public static IEnumerable<UserAddressDto> Create(List<UserAddressEntity> entities)
    {
        if (entities == null || entities.Count == 0)
        {
            return null!;
        }
        var dtos = new List<UserAddressDto>();
        foreach (var entity in entities)
        {
            dtos.Add(Create(entity));
        }
        return dtos;
    }
        

}
