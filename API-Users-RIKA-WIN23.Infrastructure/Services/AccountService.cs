using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace API_Users_RIKA_WIN23.Infrastructure.Services
{
    public class AccountService(UserManager<UserEntity> userManager, DataContext context, ProfileService profileService, AddressService addressService)
    {
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly DataContext _context = context;
        private readonly ProfileService _profileService = profileService;
        private readonly AddressService _addressService = addressService;


        #region Create
        public async Task<ResponseResult> CreateOneUserAsync(SignUpDto newUserDto)
        {
            var errorMessage = ", errors occured when creating; ";
            var errors = false;
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var existingUser = await _userManager.FindByEmailAsync(newUserDto.Email);
                if (existingUser != null)
                {
                    return ResponseFactory.Exists("Email is already in use");
                }

                var newUserEntity = new UserEntity()
                {
                    Email = newUserDto.Email,
                    UserName = newUserDto.Email,
                };

                var result = await _userManager.CreateAsync(newUserEntity, newUserDto.Password);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(newUserDto.Email);

                    if (user != null)
                    {
                        //make securityKey the id for the given role we want to assign. Alternatively make a dictionary with key:roles corresponding to a value:string and select from that.
                        var securityKeyRole = newUserDto.SecurityKey == "admin" ? "admin" : "user";     
                        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == securityKeyRole);
                        var userRole = new IdentityUserRole<string> { RoleId = role!.Id, UserId = user.Id };
                        if (role != null && !string.IsNullOrEmpty(role.Id))
                        {
                            _context.UserRoles.Add(userRole);
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

                        if (_context.Entry(userRole).State != EntityState.Unchanged)
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
                        
                        if (errors)
                        {
                            await transaction.RollbackAsync();
                            await _userManager.DeleteAsync(user);
                            return ResponseFactory.InternalServerError($"User could not be created: {errorMessage}. Please try again, contact customer service if issue persists.");
                        }
                    }
                }

                await transaction.CommitAsync();
                return ResponseFactory.Created($"User: {newUserDto.Email}, was created succesfully");

            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"User could not be created or was created with errors: {errorMessage}, {ex.Message}");
            }

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
}
