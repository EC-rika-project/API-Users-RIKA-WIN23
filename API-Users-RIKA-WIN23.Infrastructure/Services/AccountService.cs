using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Users_RIKA_WIN23.Infrastructure.Services
{
    public class AccountService(UserManager<UserEntity> userManager, DataContext context)
    {
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly DataContext _context = context;

        #region CreateUserProfile
        public async Task<ResponseResult> CreateUserProfileAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return ResponseFactory.NotFound();
            }

            var userProfile = new UserProfileEntity
            {
                UserId = user.Id,
                Email = user.Email!
            };
            try
            {
                _context.Profiles.Add(userProfile);
                await _context.SaveChangesAsync();
                return ResponseFactory.Created();
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to save user profile: {ex.Message}");
            }         

        }
        #endregion

        #region GetProfile
        public async Task<ResponseResult> GetUserProfileAsync(string Id)
        {
            try
            {
                var result = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == Id);
                if (result == null)
                {
                    return ResponseFactory.NotFound();
                }

                var userProfileDto = UserProfileFactory.Create(result);

                return ResponseFactory.Ok(userProfileDto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to fetch user profile: {ex.Message}");
            }
            
        }
        #endregion

        // Get all users? 

        public async Task<ResponseResult> UpdateUserProfileAsync(UserProfileDto updatedDto)
        {
            try
            {
                var userProfile = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == updatedDto.UserId);
                var user = await _userManager.FindByIdAsync(updatedDto.UserId);

                if (userProfile == null || user == null)
                {
                    return ResponseFactory.NotFound();
                }

                var updatedProfile = UserProfileFactory.Create(updatedDto);
                updatedProfile.User = user;
                _context.Profiles.Entry(userProfile).CurrentValues.SetValues(updatedProfile);
                await _context.SaveChangesAsync();
        
                return ResponseFactory.Ok(UserProfileFactory.Create(updatedProfile));
            }
            
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to update user profile: {ex.Message}");
            }
        
        }
    }
}
