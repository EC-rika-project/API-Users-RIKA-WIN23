using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AuthService(SignInManager<UserEntity> signInManager)
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<UserDto> SignInUserAsync(SignInDto user)
    {
        try
        {
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                if (result.Succeeded)
                {
                    // Logic for fetching the user from repository
                    return new UserDto();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return null!;
    }
}