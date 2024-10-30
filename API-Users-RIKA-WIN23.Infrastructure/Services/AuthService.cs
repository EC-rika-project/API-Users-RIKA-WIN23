using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AuthService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

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

    public async Task<bool> SignUpUserAsync(SignUpDto user)
    {
        try
        {
            if (user != null)
            {
                //Checks for a user with the supplied email, returns false.
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return false;
                }

                //Create a UserFactory for conversion
                var entity = new UserEntity()
                {
                    Email = user.Email,
                    UserName = user.Email,
                };

                var result = await _userManager.CreateAsync(entity, user.Password);
                if (result.Succeeded)
                {

                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return false;
    }
}