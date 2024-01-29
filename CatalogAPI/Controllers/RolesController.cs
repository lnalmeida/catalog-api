using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exception = System.Exception;

namespace CatalogAPI.Controllers;

[Route("api/auth/[controller]")]
[ApiController]

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
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult> CreateRole([FromBody]string roleName)
    {
        try
        {
            string lowRoleName = roleName.ToLower();
            if (string.IsNullOrEmpty(lowRoleName))
            {
                return BadRequest("Role name cannot be empty.");
            }
                    
            if (await _roleManager.RoleExistsAsync(lowRoleName))
            {
                return BadRequest("This role already exists!");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(lowRoleName));
            return result.Succeeded
                ? Ok($"Role {lowRoleName} created successfully.")
                : BadRequest($"Failure on create role {lowRoleName}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
        
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("{roleName}")]
    public async Task<ActionResult> DeleteRole(string roleName)
    {
        try
        {
            var lowRoleName = roleName.ToLower();
            var role = await _roleManager.FindByNameAsync(lowRoleName);

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
    
    [Authorize(Roles = "admin")]
    [HttpPatch("change/{userName}")]
    public async Task<ActionResult> ChangeUserRole(string userName, [FromBody]string newRole)
    {
        try
        {
            string lowNewRole = newRole.ToLower();
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user is null) return NotFound("User not found");
            var existingRole = await _roleManager.FindByNameAsync(lowNewRole);
            if (existingRole is null) return BadRequest("This role not exists!");
            var currentRoles = _userManager.GetRolesAsync(user).Result.ToList();
            foreach (var role in currentRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            await _userManager.AddToRoleAsync(user, lowNewRole);
            return Ok($"The user {user.Email} now have a new role: {lowNewRole}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
    }
}