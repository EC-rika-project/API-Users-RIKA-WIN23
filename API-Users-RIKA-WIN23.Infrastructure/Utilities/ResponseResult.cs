
namespace API_Users_RIKA_WIN23.Infrastructure.Utilities;

public enum StatusCode
{
    OK = 200,
    CREATED = 201,
    ERROR = 400,
    UNAUTHORIZED = 401,
    NOT_FOUND = 404,
    EXISTS = 409    
}

public class ResponseResult
{
    public StatusCode StatusCode { get; set;}
    public object? ContentResult { get; set;}
    public string? Message { get; set;}
}
