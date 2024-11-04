using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;

namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AuthService(SignInManager<UserEntity> signInManager, AccountService accountService)
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly AccountService _accountService = accountService;

    public async Task<ResponseResult> SignInUserAsync(SignInDto signInDto)
    {
        try
        {
            var signInResult = await _signInManager.PasswordSignInAsync(signInDto.Email, signInDto.Password, signInDto.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return await _accountService.GetOneUserAsync(signInDto.Email);
            }

            return ResponseFactory.Error($"Sign in failed, check your credentials and try again.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"User could not be signed in: {ex.Message}");
        }
    }
}