using Microsoft.IdentityModel.Tokens;
using SquadronApi.Data;
using SquadronApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SquadronApi.Services;

public class TokenService
{
    private readonly DataContext _context;
    private readonly IConfiguration _config;

    public TokenService(DataContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> CreateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Name, user.DisplayName)
        };

        // If we add roles add them in the token as well

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:TokenKey"]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials,
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"]);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
