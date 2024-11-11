using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;

namespace API_Users_RIKA_WIN23.Infrastructure.Interfaces;

public interface IAddressService
{
    Task<ResponseResult> CreateUserAddressAsync(string id);
    Task<ResponseResult> DeleteUserAddressAsync(string userId);
    Task<ResponseResult> GetAllUserAddressesAsync(int count);
    Task<ResponseResult> GetOneUserAddressAsync(string userId);
    Task<ResponseResult> UpdateUserAddressAsync(UserAddressDto updatedAddressDto);
}