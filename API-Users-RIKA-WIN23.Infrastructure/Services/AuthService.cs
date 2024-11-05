using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Interfaces;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;

namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AuthService(SignInManager<UserEntity> signInManager, AccountService accountService) : IAuthService
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
                //This should probably return a created JWT Token with the correct Claims that the client app can then create a cookie with.
                //Which in turn identity can create a ClaimsPrincipal from.
                return ResponseFactory.Ok();
            }

            return ResponseFactory.Error($"Sign in failed, check your credentials and try again.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"User could not be signed in: {ex.Message}");
        }
    }
}