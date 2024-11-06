using API_Users_RIKA_WIN23.Filters;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API_Users_RIKA_WIN23.Controllers;

[ApiKey]
[Route("api/[controller]")]
[ApiController]
public class ProfileController(StatusCodeGenerator statusCodeGenerator, ProfileService profileService) : ControllerBase
{
    private readonly StatusCodeGenerator _statusCodeGenerator = statusCodeGenerator;
    private readonly ProfileService _profileService = profileService;

    #region User Profile Endpoints
    // We should check here for permissions and only let admins access this endpoint.
    [HttpPost]
    public async Task<IActionResult> CreateUserProfileAsync(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            var result = await _profileService.CreateUserProfileAsync(email);
            return _statusCodeGenerator.HttpSelector(result);
        }
        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfileAsync(string userId)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.GetUserProfileAsync(userId);
            return _statusCodeGenerator.HttpSelector(result);
        }
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserProfileAsync(UserProfileDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.UpdateUserProfileAsync(dto);
            return _statusCodeGenerator.HttpSelector(result);
        }
        return BadRequest();
    }

    // We should check here for permissions and only let admins access this endpoint.
    [HttpDelete]
    public async Task<IActionResult> DeleteUserProfileAsync(string userId)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.DeleteUserProfileAsync(userId);
            return _statusCodeGenerator.HttpSelector(result);
        }
        return BadRequest();
    }
    #endregion
}
