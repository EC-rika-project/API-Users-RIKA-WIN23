using API_Users_RIKA_WIN23.Filters;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API_Users_RIKA_WIN23.Controllers;

[ApiKey]
[Route("api/[controller]")]
[ApiController]
public class AddressController(AddressService addressService, StatusCodeGenerator statusCodeGenerator) : ControllerBase
{
    private readonly AddressService _addressService = addressService;
    private readonly StatusCodeGenerator _statusCodeGenerator = statusCodeGenerator;

    [HttpPost]
    public async Task<IActionResult> CreateAddressAsync(string userId)
    {
        if (userId != null)
        {
            var result = await _addressService.CreateUserAddressAsync(userId);
            return _statusCodeGenerator.HttpSelector(result);
        }

        return BadRequest();
    }

    [Route ("/api/Address/{userId}")]
    [HttpGet]
    public async Task<IActionResult> GetAddressAsync(string userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            var result = await _addressService.GetOneUserAddressAsync(userId);
            return _statusCodeGenerator.HttpSelector(result);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAddressesAsync(int count = 0)
    {
        if (count >= 0)
        {
            var result = await _addressService.GetAllUserAddressesAsync(count);
            return _statusCodeGenerator.HttpSelector(result);
        }
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAddressAsync(UserAddressDto updatedAddressDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _addressService.UpdateUserAddressAsync(updatedAddressDto);
            return _statusCodeGenerator.HttpSelector(result);
        }

        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAddressAsync(string userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            var result = await _addressService.DeleteUserAddressAsync(userId);
            return _statusCodeGenerator.HttpSelector(result);
        }

        return BadRequest();
    }
}
