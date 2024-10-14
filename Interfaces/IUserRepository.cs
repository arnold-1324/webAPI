using System;
using twitterclone.Models;
namespace twitterclone.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUsernameOrEmailAsync(string username, string email);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<User?> GetUserByIdAsync(string userId);
}

