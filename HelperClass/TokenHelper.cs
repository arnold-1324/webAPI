using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using twitterclone.Models;
using System.Collections.Concurrent;

namespace twitterclone.HelperClass;

public class TokenHelper
{
    
 private readonly string _secretKey;
 private readonly string _issuer;

 private readonly string _subject;
 
 private readonly string _audience;

private static readonly ConcurrentDictionary<string, DateTime> RevokedTokens = new ConcurrentDictionary<string, DateTime>();
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
            new Claim("userID", user.Id?.ToString())
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


   public async Task<bool> RevokeTokenAsync(string token)
{
    if (string.IsNullOrEmpty(token))
        return false;

    try
    {
       
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var expiry = jwtToken.ValidTo;

        
        RevokedTokens[token] = expiry;

       
        return true;
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"Error revoking token: {ex.Message}");
        return false;
    }
}

       public bool IsTokenRevoked(string token)
{
    if (string.IsNullOrEmpty(token))
        return true;

    
    if (RevokedTokens.TryGetValue(token, out var expiry))
    {
        
        if (expiry <= DateTime.UtcNow)
        {
            RevokedTokens.TryRemove(token, out _); 
            return false;
        }

        return true;
    }

    return false; 
}


}
