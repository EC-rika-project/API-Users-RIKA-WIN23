using API_Users_RIKA_WIN23.Filters;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API_Users_RIKA_WIN23.Controllers
{
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(StatusCodeGenerator statusCodeSelector, AccountService accountService) : ControllerBase
    {
        private readonly StatusCodeGenerator _statusCodeGenerator = statusCodeSelector;
        private readonly AccountService _accountService = accountService;

        #region Post
        [Route("/api/SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUpAsync(SignUpDto user)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.CreateOneUserAsync(user);
                return _statusCodeGenerator.HttpSelector(result);
            }

            return BadRequest();
        }
        #endregion

        #region Get
        //Only Accessible with proper token
        [UserJwtReq]
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

        //Is now admin only endpoint
        [AdminJwtReq]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(int count = 0)
        {
            if (count >= 0)
            {
                var result = await _accountService.GetAllUsersAsync(count);
                return _statusCodeGenerator.HttpSelector(result);                
            }
            return BadRequest();
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
