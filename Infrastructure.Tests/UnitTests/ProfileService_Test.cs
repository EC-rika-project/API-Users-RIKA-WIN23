

using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Interfaces;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Moq;

namespace Infrastructure.Tests.UnitTests;

public class ProfileService_Test
{
    private Mock<IProfileService> _profileServiceMock;
    public ProfileService_Test()
    {
        _profileServiceMock = new Mock<IProfileService>();
    }

    [Fact]
    public async void CreateUserProfileAsync_ShouldCreateUserProfile_AndReturnStatusCode_Created()
    {
        //Arrange
        var userProfile = new SignUpDto { Email = "test@domain.com", FirstName = "Test", LastName = "Testsson"};
        ResponseResult expectedResult = ResponseFactory.Created("User profile created");

        _profileServiceMock.Setup(x => x.CreateUserProfileAsync(userProfile)).ReturnsAsync(expectedResult);

        //Act

        ResponseResult result = await _profileServiceMock.Object.CreateUserProfileAsync(userProfile);

        //Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedResult.Message, result.Message);
    }

}
