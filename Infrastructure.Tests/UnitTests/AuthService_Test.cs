

using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Interfaces;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Infrastructure.Tests.UnitTests;

public class AuthService_Test
{

    private Mock<IAuthService> _authServiceMock;

    public AuthService_Test()
    {
        _authServiceMock = new Mock<IAuthService>();
    }

    [Fact]
    public async void LoginUserAsync_ShouldLogInUser_AndReturnUserDto()
    {
        //Arrange
        var user = new SignInDto { Email = "test@domain.com", Password = "BytMig123!", RememberMe = false };
        ResponseResult expectedResult = ResponseFactory.Ok();

        _authServiceMock.Setup(x => x.SignInUserAsync(user)).ReturnsAsync(expectedResult);

        //Act

        ResponseResult result = await _authServiceMock.Object.SignInUserAsync(user);

        //Assert
        Assert.Equal(expectedResult, result);
    }


    [Fact]
    public async void FailedToLoginUserAsync_ShouldNotLogInUser_AndReturnError()
    {
        //Arrange
        var user = new SignInDto { Email = "test@domain.com", Password = "BytMig123!", RememberMe = false };
        ResponseResult expectedResult = ResponseFactory.Error("Failed");

        _authServiceMock.Setup(x => x.SignInUserAsync(user)).ReturnsAsync(expectedResult);

        //Act

        ResponseResult result = await _authServiceMock.Object.SignInUserAsync(user);

        //Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedResult.Message, result.Message);
    }
}
