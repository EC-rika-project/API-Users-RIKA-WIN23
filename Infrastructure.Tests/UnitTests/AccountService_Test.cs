using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Interfaces;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Infrastructure.Tests.UnitTests;

public class AccountService_Test
{
    private Mock<IAccountService> _accountServiceMock;
    //private Mock<IConfiguration> _configurationMock;
    private readonly DataContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly AccountService _accountService;
    private readonly IConfiguration _configuration;


    public AccountService_Test()
    {
        var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options;
        _context = new DataContext(options);
        var userStoreMock = new Mock<IUserStore<UserEntity>>();
        _userManager = new UserManager<UserEntity>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        
        _accountServiceMock = new Mock<IAccountService>();
        //_configurationMock = new Mock<IConfiguration>();
        _accountService = new AccountService(_userManager, _context, _configuration);

    }

    [Fact]
    public async void GetOneUserAsync_ShouldGetOneUserDtoByEmail_AndReturnResponseResultOk()
    {
        //Arrange
        var email = "test@test.com";
        var userDto = new UserDto { UserName = email };
        ResponseResult expectedResult = ResponseFactory.Ok(userDto);

        _accountServiceMock.Setup(x => x.GetOneUserAsync(email)).ReturnsAsync(expectedResult);

        //Act
        ResponseResult result = await _accountServiceMock.Object.GetOneUserAsync(email);
        var fetchedDto = (UserDto)result.ContentResult!;

        //Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(email, fetchedDto.UserName);
    }

    [Fact]
    public async void GetOneUserAsync_ShouldNotGetOneUserDtoByEmail_AndReturnResponseResultNotFound()
    {
        //Arrange
        var email = "felemail@test.com";
        ResponseResult expectedResult = ResponseFactory.NotFound($"There is no user with email address: {email} in database.");

        _accountServiceMock.Setup(x => x.GetOneUserAsync(email)).ReturnsAsync(expectedResult);

        //Act
        ResponseResult result = await _accountServiceMock.Object.GetOneUserAsync(email);


        //Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedResult.Message, result.Message);
    }

    [Fact]

    public async void GetAllUserAsync_ShouldNotGetAllUsers_AndReturnResponseResultNotFound()
    {
        //Arrange
               
        ResponseResult expectedResult = ResponseFactory.NotFound("No users in database");


        //Act
        ResponseResult result = await _accountService.GetAllUsersAsync(0);


        //Assert
        Assert.Equal(expectedResult.StatusCode, result.StatusCode);
        Assert.Equal(expectedResult.Message, result.Message);
    }
}


