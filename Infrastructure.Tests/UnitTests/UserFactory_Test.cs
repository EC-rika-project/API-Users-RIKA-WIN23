using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;

namespace Infrastructure.Tests.UnitTests;

public class UserFactory
{
    [Fact]
    public void ConvertUserEntityToUserDto_ShouldKeepTheSameValuesForEachProperty_AndReturnAUserDto()
    {
        //Arrange
        UserEntity userEntity = new UserEntity()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "test@test.com",
            IsExternalAccount = false,
            Profile = null,
            Address = null,
            WishList = null,
            ShoppingCarts = null,
        };

        //Act
        var userDto = API_Users_RIKA_WIN23.Infrastructure.Factories.UserFactory.Create(userEntity);

        //Assert
        Assert.IsType<UserDto>(userDto);
        Assert.Equal(userEntity.Id, userDto.Id);
        Assert.Equal("test@test.com", userDto.UserName);
        Assert.Equal(userEntity.UserName, userDto.UserName);
        Assert.Equal(userEntity.IsExternalAccount, userDto.IsExternalAccount);
        Assert.Null(userDto.Profile);
        Assert.Null(userDto.Address);
        Assert.Null(userDto.WishList);
        Assert.Null(userDto.ShoppingCarts);
    }

    [Fact]
    public void ConvertUserDtoToUserEntity_ShouldKeepTheSameValuesForEachProperty_AndReturnAUserEntity()
    {
        //Arrange
        UserDto userDto = new UserDto()
        {
            Id = Guid.NewGuid().ToString(),
            IsExternalAccount = false,
            Profile = null,
            Address = null,
            WishList = null,
            ShoppingCarts = null,
        };

        //Act
        var userEntity = API_Users_RIKA_WIN23.Infrastructure.Factories.UserFactory.Create(userDto);

        //Assert
        Assert.IsType<UserEntity>(userEntity);
        Assert.Equal(userEntity.Id, userDto.Id);
        Assert.Equal(userEntity.IsExternalAccount, userDto.IsExternalAccount);
        Assert.Null(userEntity.Profile);
        Assert.Null(userEntity.Address);
        Assert.Null(userEntity.WishList);
        Assert.Null(userEntity.ShoppingCarts);
    }
}
