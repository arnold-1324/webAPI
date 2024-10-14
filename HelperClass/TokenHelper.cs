using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using twitterclone.Models;

namespace twitterclone.HelperClass;

public class TokenHelper
{
 private readonly string _secretKey;

    public TokenHelper(string secretKey)
    {
        _secretKey = secretKey;
    }

    public string GenerateToken(User user)
    {
        
        var expiration = DateTime.UtcNow.AddHours(1); 
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

       
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

       
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

       
        var token = new JwtSecurityToken(
            issuer: "Adminstrator", 
            audience: "User", 
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

     
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
