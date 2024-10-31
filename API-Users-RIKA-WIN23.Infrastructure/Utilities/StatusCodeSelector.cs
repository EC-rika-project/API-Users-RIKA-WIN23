using Microsoft.AspNetCore.Mvc;


namespace API_Users_RIKA_WIN23.Infrastructure.Utilities;

public class StatusCodeSelector : ControllerBase
{
    /// <summary>
    /// Genereates a http Statusmessage from correspponding ResponseResult.
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public IActionResult StatusSelector(ResponseResult response)
    {
        switch (response.StatusCode)
        {
            case Infrastructure.Utilities.StatusCode.OK:
                return Ok(response.ContentResult ?? null);
            case Infrastructure.Utilities.StatusCode.CREATED:
                return Created(response.Message ?? "Resource Created", response.ContentResult ?? null);
            case Infrastructure.Utilities.StatusCode.UNAUTHORIZED:
                return Unauthorized(response.Message ?? "You Are Trying To Access Unauthorized content.");
            case Infrastructure.Utilities.StatusCode.NOT_FOUND:
                return NotFound(response.Message ?? "Resource Not Found");
            case Infrastructure.Utilities.StatusCode.EXISTS:
                return Conflict(response.Message ?? "Resource Already Exists");
            default:
                return StatusCode(500, response.Message ?? "Internal Server Error");
        }
    }
}
