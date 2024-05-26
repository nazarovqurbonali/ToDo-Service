using System.Net;
using Domain.DTOs.UserDTOs;
using Domain.Filters;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase

{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res1 = await userService.GetUsersAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var res1 = await userService.GetUserByIdAsync(userId);
        return StatusCode(res1.StatusCode, res1);
    }


    [HttpPut("update")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUser)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await userService.UpdateUserAsync(updateUser, userId);
        return StatusCode(result.StatusCode, result);
    }
}