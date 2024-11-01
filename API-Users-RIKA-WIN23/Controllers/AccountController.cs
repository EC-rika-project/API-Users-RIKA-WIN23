using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API_Users_RIKA_WIN23.Controllers
{
    // Create DataAnnotation for APIkey in initialize here.
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(StatusCodeGenerator statusCodeSelector, AccountService accountService) : ControllerBase
    {
        private readonly StatusCodeGenerator _statusCodeGenerator = statusCodeSelector;
        private readonly AccountService _accountService = accountService;

        #region Post
        //Create endpoint is located in authcontroller as a HttpPost SignUp.
        #endregion

        #region Get
        [Route("/api/Account/{email}")]
        [HttpGet]
        public async Task<IActionResult> GetUserAsync(string email)
        {
            if (email != null)
            {
                var result = await _accountService.GetOneUserAsync(email);
                return _statusCodeGenerator.HttpSelector(result);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _accountService.GetAllUsersAsync();
            return _statusCodeGenerator.HttpSelector(result);
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UserDto updatedUserDto)
        {
            var result = await _accountService.UpdateUserAsync(updatedUserDto);
            return _statusCodeGenerator.HttpSelector(result);
        }
        #endregion

        #region Delete
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var result = await _accountService.DeleteUserAsync(id);
            return _statusCodeGenerator.HttpSelector(result);
        }
        #endregion
    }
}
