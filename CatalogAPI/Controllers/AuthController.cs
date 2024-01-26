using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CatalogAPI.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CatalogAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        try
        {
            return $"AuthController :: Accessed in: {DateTime.Now.ToLongDateString()}";
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(UserRegisterDto userModel)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var newUser = new IdentityUser
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                EmailConfirmed = true,
            };
            
            var result = await _userManager.CreateAsync(newUser, userModel.Password);
            await _userManager.AddToRoleAsync(newUser, userModel.Role);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _signInManager.SignInAsync(newUser, false);
            return Ok(userModel);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }

    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto userInfo)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password,
                isPersistent: false,
                lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(userInfo.Email);
                var role = await _userManager.GetRolesAsync(user);

                var userRole = role.FirstOrDefault();
                if (userRole != null) return Ok(GenerateToken(userInfo, userRole.ToString()));
            }
            else
            {
                ModelState.AddModelError("Errors", "Invalid login!");
                return BadRequest(ModelState);
            }

            return null;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred with your request. \nMessage: {ex.Message}");
        }
       
    }

    private UserTokenDto GenerateToken(LoginDto userInfo, string? role)
    {
        try
        {
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                    new Claim("nome", "L.Nunes"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var expiresIn = DateTime.UtcNow.AddHours(double.Parse(_configuration["TokenConfiguration:ExpireHours"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiresIn,
                signingCredentials: credentials
            );

            return new UserTokenDto()
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiiration = expiresIn,
                Message = "JWT Token OK"
            };
        }
        catch (Exception ex)
        {
            throw  new Exception($"An error occurred with your request. \nMessage: {ex.Message}");
        }
    }
}