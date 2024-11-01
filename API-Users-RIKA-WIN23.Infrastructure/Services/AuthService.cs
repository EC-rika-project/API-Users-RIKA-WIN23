using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AuthService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, AccountService accountService, ProfileService profileService)
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AccountService _accountService = accountService;
    private readonly ProfileService _profileService = profileService;

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

    public async Task<ResponseResult> SignUpUserAsync(SignUpDto newUser)
    {
        try
        {
            if (newUser != null)
            {
                //Checks for a user with the supplied email, returns false.
                var existingUser = await _userManager.FindByEmailAsync(newUser.Email);
                if (existingUser != null)
                {
                    return ResponseFactory.Exists("Email is already in use");
                }

                //Create a UserFactory for conversion
                var entity = new UserEntity()
                {
                    Email = newUser.Email,
                    UserName = newUser.Email,
                };

                var result = await _userManager.CreateAsync(entity, newUser.Password);
                if (result.Succeeded)
                {
                    var profile = await _profileService.CreateUserProfileAsync(newUser);
                    if (profile.StatusCode == StatusCode.CREATED)
                    {
                        return profile;
                    }
                    return ResponseFactory.Created("User is created but an error occurred when creating profile");
                }

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return ResponseFactory.InternalServerError();
    }
}