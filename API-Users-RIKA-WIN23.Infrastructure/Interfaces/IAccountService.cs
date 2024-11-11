using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;

namespace API_Users_RIKA_WIN23.Infrastructure.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseResult> CreateOneUserAsync(SignUpDto newUserDto);
        Task<ResponseResult> DeleteUserAsync(string userId);
        Task<ResponseResult> GetAllUsersAsync(int count);
        Task<ResponseResult> GetOneUserAsync(string email);
        Task<ResponseResult> UpdateUserAsync(UserDto updatedUserDto);
    }
}