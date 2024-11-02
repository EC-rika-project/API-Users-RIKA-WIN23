using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AddressService(UserManager<UserEntity> userManager, DataContext dataContext)
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly DataContext _context = dataContext;

    public async Task<ResponseResult> CreateUserAddressAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return ResponseFactory.NotFound();
        }

        var existingAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (existingAddress != null)
        {
            return ResponseFactory.Exists("User address already exists in database.");
        }

        var address = new UserAddressEntity
        {
            UserId = user.Id,
        };

        try
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return ResponseFactory.Created("User address created.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"Failed to create user address: {ex.Message}");
        }
    }

    public async Task<ResponseResult> GetOneUserAddressAsync(string userId)
    {
        try
        {
            var existingAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existingAddress == null)
            {
                return ResponseFactory.NotFound($"No address with submitted id: {userId} was found in database");
            }

            var addressDto = AddressFactory.Create(existingAddress);
            return ResponseFactory.Ok(addressDto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"Failed to fetch user address from database: {ex.Message}");
        }
    }

    public async Task<ResponseResult> GetAllUserAddressesAsync(int count)
    {
        try
        {
            var existingAddresses = new List<UserAddressEntity>();
            if (count > 0)
            {
                existingAddresses = await _context.Addresses.Take(count).ToListAsync();
            }
            else
            {
                existingAddresses = await _context.Addresses.ToListAsync();
            }

            if (existingAddresses != null && existingAddresses.Count() > 0)
            {
                var addressDtos = AddressFactory.Create(existingAddresses);
                if (addressDtos != null && addressDtos.Count() > 0)
                {
                    return ResponseFactory.Ok(addressDtos);
                }
            }

            return ResponseFactory.NotFound("No addresses found in database");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"Failed to fetch user address from database: {ex.Message}");
        }
    }

    public async Task<ResponseResult> UpdateUserAddressAsync(UserAddressDto updatedAddressDto)
    {
        var existingAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == updatedAddressDto.UserId);
        if (existingAddress == null)
        {
            return ResponseFactory.NotFound("No address found in database");
        }

        var updatedAddress = AddressFactory.Create(updatedAddressDto);

        try
        {  
            _context.Addresses.Entry(existingAddress).CurrentValues.SetValues(updatedAddress);
            await _context.SaveChangesAsync();
            updatedAddressDto = AddressFactory.Create(updatedAddress);
            return ResponseFactory.Ok(updatedAddressDto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"Failed to update user address in database: {ex.Message}");
        }
    }

    public async Task<ResponseResult>DeleteUserAddressAsync(string userId)
    {
        try
        {
            var existingAddresses = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existingAddresses == null)
            {
                return ResponseFactory.NotFound("No address found in database");
            }

            _context.Addresses.Remove(existingAddresses);
            await _context.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"Failed to update user address in database: {ex.Message}");
        }
    }

}
