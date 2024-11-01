using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace API_Users_RIKA_WIN23.Infrastructure.Services
{
    public class AccountService(UserManager<UserEntity> userManager, DataContext context)
    {
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly DataContext _context = context;

        #region Create
        //Create user method is located in AuthService as SignUpUserAsync.
        #endregion

        #region Read
        public async Task<ResponseResult> GetOneUserAsync(string email)
        {
            try
            {
                var userEntity = await _context.Users
                    .Include(x => x.Profile)
                    .Include(x => x.Address)
                    .Include(x => x.WishList)
                    .Include(x => x.ShoppingCarts)
                    .FirstOrDefaultAsync(x => x.UserName == email);

                if (userEntity == null)
                {
                    return ResponseFactory.NotFound($"There is no user with email address: {email} in database.");
                }

                var userDto = UserFactory.Create(userEntity);
                return ResponseFactory.Ok(userDto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to get user: {ex.Message}");
            }
        }

        public async Task<ResponseResult> GetAllUsersAsync()
        {
            try
            {
                var userEntities = await _context.Users.ToListAsync();
                if (userEntities.Count() == 0)
                {
                    return ResponseFactory.NotFound("No users in database");
                }

                var userDtos = UserFactory.Create(userEntities);
                return ResponseFactory.Ok(userDtos);
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to get users: {ex.Message}");
            }
        }
        #endregion

        #region Update
        public async Task<ResponseResult> UpdateUserAsync(UserDto updatedUserDto)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(x => x.Profile)
                    .Include(x => x.Address)
                    .Include(x => x.WishList)
                    .Include(x => x.ShoppingCarts)
                    .FirstOrDefaultAsync(x => x.Id == updatedUserDto.Id);
                if (existingUser == null)
                {
                    return ResponseFactory.NotFound($"There is no user with Id: {updatedUserDto.Id} in database.");
                }

                if (updatedUserDto.Profile != null)
                {
                    existingUser.Profile= UserProfileFactory.Create(updatedUserDto.Profile);
                }
                if (updatedUserDto.Address != null)
                {
                    existingUser.Address = AddressFactory.Create(updatedUserDto.Address);
                }
                if (updatedUserDto.WishList != null)
                {
                    existingUser.WishList = WishListFactory.Create(updatedUserDto.WishList);
                }
                if (updatedUserDto.ShoppingCarts != null && updatedUserDto.ShoppingCarts.Count() > 0)
                {
                    existingUser.ShoppingCarts = ShoppingCartFactory.Create(updatedUserDto.ShoppingCarts);
                }

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    var updatedUserDtoResponse = UserFactory.Create(existingUser);
                    return ResponseFactory.Ok(updatedUserDtoResponse);
                }

                return ResponseFactory.InternalServerError($"Failed to update user in: _userManager.UpdateAsync(updatedUserEntity)");
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to update user: {ex.Message}");
            }
        }
        #endregion

        #region Delete
        public async Task<ResponseResult> DeleteUserAsync(string id)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return ResponseFactory.NotFound($"There is no user with Id: {id} in database.");
                }

                var result = await _userManager.DeleteAsync(existingUser);
                if (result.Succeeded)
                {
                    return ResponseFactory.Ok($"User with Id: {id}, was deleted succesfully.");
                }
                return ResponseFactory.InternalServerError($"Failed to delete user in method: _userManager.DeleteAsync(existingUser)");
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to delete user: {ex.Message}");
            }
        }
        #endregion
    }
}
