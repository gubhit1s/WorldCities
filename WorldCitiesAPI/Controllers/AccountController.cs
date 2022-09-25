using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace WorldCitiesAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtHandler _jwtHandler;
    
    public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, JwtHandler jwtHandler) {
        _context = context;
        _userManager = userManager;
        _jwtHandler = jwtHandler;
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(AccountInfo loginRequest) {
        var user = await _userManager.FindByNameAsync(loginRequest.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Unauthorized(new LoginResult() {
                Success = false,
                Message = "Invalid Email or Password."
            });
        var secToken = await _jwtHandler.GetTokenAsync(user);
        var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
        return Ok(new LoginResult() {
            Success = true, Message = "Login successful", Token = jwt
        });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(AccountInfo registerRequest)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(registerRequest.Email);
        if (user != null)
        {
            return BadRequest(new RegisterResult()
            {
                Success = false,
                Message = "Registration failed, this account has already been registered!"
            });
        }
        else
        {
            ApplicationUser newUser = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerRequest.Email,
                Email = registerRequest.Email,
            };

            await _userManager.CreateAsync(newUser, registerRequest.Password);
            await _userManager.AddToRoleAsync(newUser, "RegisteredUser");

            return Ok(new RegisterResult()
            {
                Success = true,
                Message = "Registration successful"
            });
        }
    }
}
