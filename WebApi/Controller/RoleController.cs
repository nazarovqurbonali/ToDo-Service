using Domain.DTOs.RoleDTOs;
using Domain.Filters;
using Infrastructure.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class RoleController(IRoleService roleService) : ControllerBase
{
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles([FromQuery] RoleFilter filter)
    {
        var response = await roleService.GetRolesAsync(filter);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{roleId:int}")]
    public async Task<IActionResult> GetRoleById(int roleId)
    {
        var response = await roleService.GetRoleByIdAsync(roleId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRole)
    {
        var result = await roleService.CreateRoleAsync(createRole);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPut("update")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto updateRole)
    {
        var result = await roleService.UpdateRoleAsync(updateRole);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{roleId:int}")]
    public async Task<IActionResult> ChangePassword(int roleId)
    {
        var result = await roleService.DeleteRoleAsync(roleId);
        return StatusCode(result.StatusCode, result);
    }
}