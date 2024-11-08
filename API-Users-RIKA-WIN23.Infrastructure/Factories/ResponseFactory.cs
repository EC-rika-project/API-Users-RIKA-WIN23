﻿
using API_Users_RIKA_WIN23.Infrastructure.Utilities;

namespace API_Users_RIKA_WIN23.Infrastructure.Factories;

public class ResponseFactory
{
    public static ResponseResult Ok(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Succeeded",
            StatusCode = StatusCode.OK
        };
    }

    public static ResponseResult Ok(object obj, string? message = null)
    {
        return new ResponseResult
        {
            ContentResult = obj,
            Message = message ?? "Succeeded",
            StatusCode = StatusCode.OK
        };
    }

    public static ResponseResult Created(object obj, string? message = null)
    {
        return new ResponseResult
        {
            ContentResult = obj,
            Message = message ?? "Created",
            StatusCode = StatusCode.CREATED
        };
    }

    public static ResponseResult Created(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Created, no content",
            StatusCode = StatusCode.CREATED
        };
    }

    public static ResponseResult Error(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Failed",
            StatusCode = StatusCode.ERROR
        };
    }

    public static ResponseResult Unauthorized(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Unauthorized",
            StatusCode = StatusCode.UNAUTHORIZED
        };
    }


    public static ResponseResult NotFound(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Not found",
            StatusCode = StatusCode.NOT_FOUND
        };
    }

    public static ResponseResult Exists(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Already exists",
            StatusCode = StatusCode.EXISTS
        };
    }

    public static ResponseResult InternalServerError (string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Internal Server Error",
            StatusCode = StatusCode.INTERNAL_SERVER_ERROR
        };
    }

}
