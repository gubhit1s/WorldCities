using Micosoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Token.Jwt;
using System.Security.Claims;
using System.Text;
using WorldCitiesAPI.Data.Models;

namespace WorldCitiesAPI.Data;
public class JwtHandler {
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public JwtHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager) {
        _configuration = configuration;
        _userManager = userManager;
    }
    
    public async Task<JwtSecurityToken> GetTokenAsync(ApplicationUser user) {
        var jwtOptions = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: await GetClaimsAsync(user),
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(
                _configuration["JwtSettings:ExpirationTimeInMinutes"])),
            signingCredentials: getSigningCredentials());
        return jwtOptions;
    }
    
    private SigningCredentials GetSigningCredentials() {
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecurityKey"]);
        var secret = new SymmetricSecurityKey(key);
        return new SiginingCredentials(secret, SecurityAlgorithms.HmcSha256);
    }
    
    private async Task<List<Claim>> GetClaimAsync(ApplicationUser user) {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Email)
        };
        
        foreach (var role in _userManager.GetRolesAsync(user)) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }
}
