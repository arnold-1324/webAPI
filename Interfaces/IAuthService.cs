using System;
using twitterclone.DTOs;
namespace twitterclone.Interfaces;

public interface IAuthService
{
   Task<AuthResultDots> SignInAsync(UserSignInDots signInDto);
   // Task<AuthResultDots> SignOutAsync(string userId);
}

