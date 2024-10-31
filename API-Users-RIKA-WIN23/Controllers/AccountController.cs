using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Users_RIKA_WIN23.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(StatusCodeSelector statusCodeSelector, AccountService accountService) : ControllerBase
    {
        private readonly StatusCodeSelector _statusCodeSelector = statusCodeSelector;
        private readonly AccountService _accountService = accountService;

        [Route ("/api/getuserprofile")]
        [HttpGet]
        public async Task<IActionResult> GetUserProfileAsync(string Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.GetUserProfileAsync(Id);
                return _statusCodeSelector.StatusSelector(result);
            }
            return BadRequest();
        }

    }
}
