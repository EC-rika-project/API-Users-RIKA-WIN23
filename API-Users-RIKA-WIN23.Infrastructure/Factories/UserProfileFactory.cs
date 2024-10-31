using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class UserProfileFactory
{
    public static UserProfileDto Create(UserProfileEntity entity)
    {
        return new UserProfileDto
        {
            UserId = entity.UserId,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            ProfileImageUrl = entity.ProfileImageUrl,
            Gender = entity.Gender,
            Age = entity.Age,
        };
    }

    /// <summary>
    /// When client sends back UserProfileDto to API we use the id to fetch the userEntity from db and send it into this function. 
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public static UserProfileEntity Create(UserProfileDto dto)
    {
        return new UserProfileEntity
        {   
            UserId = dto.UserId,
            User = new UserEntity(),
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            ProfileImageUrl = dto.ProfileImageUrl ?? null!,
            Gender = dto.Gender,
            Age = dto.Age,
        };
    }
}
