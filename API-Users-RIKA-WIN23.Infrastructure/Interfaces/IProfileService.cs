using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;

namespace API_Users_RIKA_WIN23.Infrastructure.Interfaces
{
    public interface IProfileService
    {
        Task<ResponseResult> CreateUserProfileAsync(SignUpDto dto);
        Task<ResponseResult> CreateUserProfileAsync(string email);
        Task<ResponseResult> DeleteUserProfileAsync(string userId);
        Task<ResponseResult> GetUserProfileAsync(string userId);
        Task<ResponseResult> UpdateUserProfileAsync(UserProfileDto updatedDto);
    }
}