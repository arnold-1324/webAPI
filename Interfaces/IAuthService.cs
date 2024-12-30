using System;
using twitterclone.DTOs;
namespace twitterclone.Interfaces;

public interface IAuthService
{
   Task<AuthResultDots> SignInAsync(UserSignInDots signInDto);
   Task<AuthResultDots> LoginAsync(LoginDots loginDto);
 
    Task<AuthResultDots> LogoutAsync(string token);
  
   // Task<AuthResultDots> SignOutAsync(string userId);
}

