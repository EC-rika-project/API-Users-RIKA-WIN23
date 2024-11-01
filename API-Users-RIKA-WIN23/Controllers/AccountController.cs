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

        #region User Profile Endpoints
        // We should check here for permissions and only let admins access this endpoint.
        [Route ("/api/userprofile/create")]
        [HttpPost]
        public async Task<IActionResult> CreateUserProfileAsync(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var result = await _accountService.CreateUserProfileAsync(email);
                return _statusCodeGenerator.HttpSelector(result);
            }
            return BadRequest();
        }
        
        [Route ("/api/userprofile/get")]
        [HttpGet]
        public async Task<IActionResult> GetUserProfileAsync(string Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.GetUserProfileAsync(Id);
                return _statusCodeGenerator.HttpSelector(result);
            }
            return BadRequest();
        }

        [Route("/api/userprofile/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserProfileAsync(UserProfileDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.UpdateUserProfileAsync(dto);
                return _statusCodeGenerator.HttpSelector(result);
            }
            return BadRequest();
        }

        // We should check here for permissions and only let admins access this endpoint.
        [Route("/api/userprofile/delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserProfileAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.DeleteUserProfileAsync(id);
                return _statusCodeGenerator.HttpSelector(result);
            }
            return BadRequest();
        }
        #endregion

        [Route ("/api/user/get")]
        [HttpGet]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            if (id != null)
            {
                var result = await _accountService.GetOneUserAsync(id);
                return _statusCodeGenerator.HttpSelector(result);
            }

            return BadRequest();
        }
    }
}
