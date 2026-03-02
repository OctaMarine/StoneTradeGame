using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.BusinessLogic.Services;

public class JwtService
{
    private readonly IOptions<AuthSettings> _options;
    
    public JwtService(IOptions<AuthSettings> options)
    {
        _options = options;
    }
    
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("userName", user.UserName),
            new Claim("email", user.Email),
            new Claim("id", user.Id.ToString())
        };
        var jwtToken = new JwtSecurityToken
        (
            expires: DateTime.Now.Add(_options.Value.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}