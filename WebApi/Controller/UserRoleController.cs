using Domain.DTOs.UserRoleDTOs;
using Domain.Filters;
using Infrastructure.Services.UserRoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class UserRoleController(IUserRoleService userUserRoleService) : ControllerBase
{
    [HttpGet("user-roles")]
    public async Task<IActionResult> GetUserUserRoles([FromQuery] PaginationFilter filter)
    {
        var res1 = await userUserRoleService.GetUserRolesAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetUserRoleById([FromQuery] UserRoleDto userRole)
    {
        var res1 = await userUserRoleService.GetUserRoleByIdAsync(userRole);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleDto createUserRole)
    {
        var result = await userUserRoleService.CreateUserRoleAsync(createUserRole);
        return StatusCode(result.StatusCode, result);
    }


    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUserRole([FromBody] UserRoleDto userRole)
    {
        var result = await userUserRoleService.DeleteUserRoleAsync(userRole);
        return StatusCode(result.StatusCode, result);
    }
}