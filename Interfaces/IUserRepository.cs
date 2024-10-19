using System;
using twitterclone.Models;
namespace twitterclone.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUsernameOrEmailAsync(string Username, string Email);
    Task<User> GetCurrentUserDetails(string Username);
    
    // Task CreateUserAsync(User user);
    // Task UpdateUserAsync(User user);
    // Task<User?> GetUserByIdAsync(string userId);
}

