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

        #region User Endpoints
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
        #endregion
    }
}
