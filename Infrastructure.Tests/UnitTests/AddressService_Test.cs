using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests.UnitTests;

public class AddressService_Test
{
    private readonly AddressService _addressService;
    private readonly DataContext _context;
    private readonly UserManager<UserEntity> _userManager;

    public AddressService_Test()
    {
        var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options;
        _context = new DataContext(options);
        var userStoreMock = new Mock<IUserStore<UserEntity>>();
        _userManager = new UserManager<UserEntity>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        _addressService = new AddressService(_userManager, _context);
    }

    [Fact]
    public async Task UpdateUserAddress_ShouldUpdateUserAddressEntity_AndReturnSuccesfulResponseResult()
    {
        //Arrange
        var userId = Guid.NewGuid().ToString();
        var initialAddress = new UserAddressEntity()
        {
            UserId = userId,
        };

        _context.Addresses.Add(initialAddress);
        await _context.SaveChangesAsync();

        var updatedUserAddress = new UserAddressDto()
        {
            UserId = userId,
            AddressLine = "Testgatan 1",
            PostalCode = 12345,
            City = "Storstan",
            Country = "Sverige"
        };

        //Act

        var updatedAddress = await _addressService.UpdateUserAddressAsync(updatedUserAddress);
        var updatedAddressDto = (UserAddressDto)updatedAddress.ContentResult!;

        //Assert

        Assert.Equal(API_Users_RIKA_WIN23.Infrastructure.Utilities.StatusCode.OK, updatedAddress.StatusCode);
        Assert.IsType<UserAddressDto>(updatedAddressDto);
        Assert.Equal(updatedUserAddress.AddressLine, updatedAddressDto.AddressLine);
        Assert.Equal(updatedUserAddress.PostalCode, updatedAddressDto.PostalCode);
        Assert.Equal(updatedUserAddress.City, updatedAddressDto.City);
        Assert.Equal(updatedUserAddress.Country, updatedAddressDto.Country);
        Assert.Equal(userId, updatedAddressDto.UserId);
    }

}
