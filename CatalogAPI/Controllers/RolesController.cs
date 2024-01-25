using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exception = System.Exception;

namespace CatalogAPI.Controllers;

[Route("api/auth/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]


public class RolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
    }
    
    [Authorize(Roles = "user")]
    [HttpPost]
    public async Task<ActionResult> CreateRole([FromBody]string roleName)
    {
        try
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }
                    
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("This role already exists!");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded
                ? Ok($"Role {roleName} created successfully.")
                : BadRequest($"Failure on create role {roleName}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
        
    }

    [HttpDelete("{roleName}")]
    public async Task<ActionResult> DeleteRole(string roleName)
    {
        try
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded ? Ok("Role deleted successfully.") : BadRequest("Failed to delete role.");
            }

            return NotFound("Role not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }

    }
    
    [HttpPatch("{username}")]
    public async Task<ActionResult> UpdateUserRole(string userName, [FromBody]string newRole)
    {
        try
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user is null) return NotFound("User not found");
            var existingRole = await _roleManager.FindByNameAsync(newRole);
            if (existingRole is null) return BadRequest("This role not exists!");
            var oldRole =  _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRoleAsync(user, oldRole.ToString());
            await _userManager.AddToRoleAsync(user, newRole);
            return Ok($"The user {user.Email} now have a new role: {newRole.ToString()}");

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
    }
}