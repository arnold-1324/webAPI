using System;
using twitterclone.DTOs;
namespace twitterclone.Interfaces;

public interface IAuthService
{
   Task<AuthResultDots> SignInAsync(UserSignInDots signInDto);

  Task<AuthResultDots> LogInAsync(UserLogin LoginDto);
   //Task<AuthResultDots> GetProfile( string userId);

   Task SendEmailAsync(EmailDto emailDto);
   // Task<AuthResultDots> SignOutAsync(string userId);
}

