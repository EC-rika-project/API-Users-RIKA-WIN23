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

        #region Get User
        public async Task<ResponseResult> GetOneUserAsync(string email)
        {
            try
            {
                var user = await _context.Users
                    .Include(x => x.Profile)
                    .Include(x => x.Address)
                    .Include(x => x.WishList)
                    .Include(x => x.ShoppingCarts)
                    .FirstOrDefaultAsync(x => x.UserName == email);

                if (user == null)
                {
                    return ResponseFactory.NotFound("There is no user with the submitted Id in database.");
                }

                var userDto = UserFactory.Create(user);
                return ResponseFactory.Ok(userDto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.InternalServerError($"Failed to get user: {ex.Message}");
            }
        }
        #endregion
    }
}
