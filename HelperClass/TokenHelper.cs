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
 private readonly string _issuer;

 private readonly string _subject;
 
 private readonly string _audience;

    public TokenHelper(string secretKey,string subject,string issuer,string audience)
    {
        _secretKey = secretKey;
        _issuer=issuer;
        _subject=subject;
        _audience=audience;
    }

    public  string GenerateToken(User user)
    {
        
        var expiration = DateTime.UtcNow.AddHours(1); 
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userID", user.Id.ToString())
        };

       
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

       
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

       
        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

     
        return  new JwtSecurityTokenHandler().WriteToken(token);
    }
}
