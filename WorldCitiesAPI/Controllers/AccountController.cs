using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Data.Models;

namespace WorldCitiesAPI.Controllers;

[Route("api/controller")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AplicationUser> _userManager;
    private readonly JwtHandler _jwtHandler;
    
    public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, JwtHandler jwtHandler) {
        _context = context;
        _userManager = userManager;
        _jwtHandler = jwtHandler;
    }
    
    [HttpPost("Login")]
    pubic async Task<IActionResult> Login(LoginRequest loginRequest) {
        var user = await _userManager.FindByNameAsync(loginRequest.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Unauthorized(new LoginResult() {
                Success = false,
                Message = "Invalid Email or Password."
            });
}
