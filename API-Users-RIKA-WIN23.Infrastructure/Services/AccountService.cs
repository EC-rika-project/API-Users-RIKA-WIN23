using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseResult> GetUserProfileAsync(string Id)
        {
            try
            {
                var result = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == Id);
                if (result == null)
                {
                    return ResponseFactory.NotFound();
                }

                var userProfileDto = new UserProfileDto
                {
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    ProfileImageUrl = result.ProfileImageUrl,
                    Gender = result.Gender,
                    Age = result.Age,
                };
                return ResponseFactory.Ok(userProfileDto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to fetch user profile: {ex.Message}");
            }
            
        }
    }
}
