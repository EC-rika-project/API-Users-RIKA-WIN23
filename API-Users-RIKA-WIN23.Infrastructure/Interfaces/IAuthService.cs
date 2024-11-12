using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;

namespace API_Users_RIKA_WIN23.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseResult> SignInUserAsync(SignInDto signInDto);
    }
}