using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace API_Users_RIKA_WIN23.Infrastructure.Services
{
    public class AccountService(UserManager<UserEntity> userManager, DataContext context)
    {
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly DataContext _context = context;

        #region Create Profile
        public async Task<ResponseResult> CreateUserProfileAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return ResponseFactory.NotFound();
            }

            var existingProfile = await _context.Profiles.FirstOrDefaultAsync(x => x.Email == user.Email || x.UserId == user.Id);
            if (existingProfile != null)
            {
                return ResponseFactory.Exists("User profile already exists in database.");
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
                return ResponseFactory.Created("User profile created.");
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to save user profile: {ex.Message}");
            }         

        }
        #endregion

        #region Get Profile
        public async Task<ResponseResult> GetUserProfileAsync(string id)
        {
            try
            {
                var result = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
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

        #region Update Profile
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
        
                return ResponseFactory.Ok(UserProfileFactory.Create(updatedProfile), "User profile updated.");
            }
            
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to update user profile: {ex.Message}");
            }
        
        }
        #endregion

        #region Delete Profile
        public async Task<ResponseResult> DeleteUserProfileAsync(string id)
        {
            try
            {
                var userProfile = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
                if (userProfile == null)
                {
                    return ResponseFactory.NotFound();
                }

                _context.Profiles.Remove(userProfile);
                await _context.SaveChangesAsync();
                return ResponseFactory.Ok("User profile deleted.");
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to delete user profile: {ex.Message}");
            }
        }

        public async Task<ResponseResult> GetOneUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return ResponseFactory.NotFound("There is no user with the submitted Id in database.");
                }

                return ResponseFactory.Ok(UserFactory.Create(user));
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to get user: {ex.Message}");
            }
        }
        #endregion
    }
}
