﻿using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AccountService(UserManager<UserEntity> userManager, DataContext context, IConfiguration configuration)
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly DataContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    #region Create
    public async Task<ResponseResult> CreateOneUserAsync(SignUpDto newUserDto)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var existingUser = await _userManager.FindByEmailAsync(newUserDto.Email);
            if (existingUser != null)
            {
                return ResponseFactory.Exists($"Email: {newUserDto.Email} is already in use");
            }

            var newUserEntity = new UserEntity()
            {
                Email = newUserDto.Email,
                UserName = newUserDto.Email,
            };

            var newUserResult = await _userManager.CreateAsync(newUserEntity, newUserDto.Password);
            if (!newUserResult.Succeeded)
            {
                return ResponseFactory.InternalServerError($"User {newUserDto.Email} could not be created. Please try again & contact customer service if issue persists.");
            }

            var newUserTables = await CreateAdditionalUserTablesAsync(newUserDto);
            if (newUserTables.Errors)
            {
                await transaction.RollbackAsync();
                if (newUserTables.User != null)
                {
                    await _userManager.DeleteAsync(newUserTables.User);
                    return ResponseFactory.InternalServerError($"User {newUserDto.Email} could not be created beacause {newUserTables.ErrorMessage} .\nPlease try again & contact customer service if issue persists.");

                }
                return ResponseFactory.InternalServerError($"User {newUserDto.Email} could not be created or was created with errors: {newUserTables.ErrorMessage}.\nPlease try again & contact customer service if issue persists.");
            }

            await transaction.CommitAsync();
            return ResponseFactory.Created($"User: {newUserDto.Email}, was created succesfully");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"User could not be created or was created with errors: {ex.Message}");
        }
    }

    private async Task<(string ErrorMessage, bool Errors, UserEntity? User)> CreateAdditionalUserTablesAsync(SignUpDto newUserDto)
    {
        var user = await _userManager.FindByEmailAsync(newUserDto.Email);

        if (user != null)
        {
            var roleResult = await AssignRoleAsync(newUserDto, user);
            if (roleResult != null)
            {
                _context.UserRoles.Add(roleResult);
            }
            else
            {
                return ("User role key not supplied, failed to create User", true, null);
            }

            var profile = new UserProfileEntity
            {
                FirstName = newUserDto.FirstName,
                LastName = newUserDto.LastName,
                UserId = user.Id,
                Email = user.Email!
            };
            var address = new UserAddressEntity
            {
                UserId = user.Id,
            };
            var wishList = new UserWishListEntity
            {
                UserId = user.Id,
            };
            var shoppingCart = new UserShoppingCartEntity
            {
                UserId = user.Id,
            };

            _context.Profiles.Add(profile);
            _context.Addresses.Add(address);
            _context.WishLists.Add(wishList);
            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();

            var result = CheckContextForErrors(roleResult!, profile, address, wishList, shoppingCart);
            return (result.ErrorMessage, result.Errors, user);
        }
        return ("User not found in database after being created", true, null);
    }

    private async Task<IdentityUserRole<string>> AssignRoleAsync(SignUpDto newUserDto, UserEntity user)
    {
        //This sets Admin role depending on which supplied key was used to set newUserDto.SecurityKey. Not ideal but a quick fix for the time being.
        var webAppKey = _configuration.GetValue<string>("WebAppKey");
        var adminAppKey = _configuration.GetValue<string>("AdminAppKey"); ;
        var userRoleName = string.Empty;

        if (newUserDto.SecurityKey == adminAppKey)
        {
            userRoleName = "admin";
        }

        if (newUserDto.SecurityKey == webAppKey)
        {
            userRoleName = "user";
        }

        if (string.IsNullOrEmpty(userRoleName))
        {
            return null!;
        }

        var securityKeyRole = newUserDto.SecurityKey == adminAppKey ? "admin" : newUserDto.SecurityKey == webAppKey ? "user" : ""; // this line will be unnecessary later when roles will be determined by actual securitykey
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == securityKeyRole);
        return new IdentityUserRole<string> { RoleId = role!.Id, UserId = user.Id };
    }

    private (string ErrorMessage, bool Errors) CheckContextForErrors(IdentityUserRole<string> roleResult, UserProfileEntity profile, UserAddressEntity address, UserWishListEntity wishList, UserShoppingCartEntity shoppingCart)
    {
        string errorMessage = ", errors occured when creating; ";
        bool errors = false;


        if (_context.Entry(roleResult).State != EntityState.Unchanged)
        {
            errorMessage += "-UserRole ";
            errors = true;
        }
        if (_context.Entry(profile).State != EntityState.Unchanged)
        {
            errorMessage += "-Profile ";
            errors = true;
        }
        if (_context.Entry(address).State != EntityState.Unchanged)
        {
            errorMessage += "-Address ";
            errors = true;
        }
        if (_context.Entry(wishList).State != EntityState.Unchanged)
        {
            errorMessage += "-WishList ";
            errors = true;
        }
        if (_context.Entry(shoppingCart).State != EntityState.Unchanged)
        {
            errorMessage += "-ShoppingCart ";
            errors = true;
        }

        return (errorMessage, errors);
    }
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
            if (userDto == null)
            {
                return ResponseFactory.InternalServerError($"Failed to convert user with email address: {email} from database entity");

            }

            var roles = new List<string>();
            foreach (var role in await _userManager.GetRolesAsync(userEntity))
            {
                roles.Add(role);
            }
            userDto.UserRoles = roles;
            return ResponseFactory.Ok(userDto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"Failed to get user: {ex.Message}");
        }
    }

    public async Task<ResponseResult> GetAllUsersAsync(int count)
    {
        try
        {
            var userEntities = new List<UserEntity>();
            if (count > 0)
            {
                userEntities = await _context.Users.Take(count).ToListAsync();

            }
            else
            {
                userEntities = await _context.Users.ToListAsync();
            }

            if (userEntities.Count() < 1)
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
                existingUser.Profile = UserProfileFactory.Create(updatedUserDto.Profile);
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
