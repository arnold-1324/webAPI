using System;
using System.Net.Http;
using System.Threading.Tasks;
using twitterclone.DTOs;
using twitterclone.Interfaces;
using twitterclone.Models;

namespace twitterclone.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;

        public UserService(IUserRepository userRepository, HttpClient httpClient)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<UserProfileDto> GetUserProfileDtoAsync(string username)
        {
            // Fetch user from the repository
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return new UserProfileDto { Success = false, Message = "User Not Found" };
            }

            


            return new UserProfileDto
            {
                Success = true,
                Profile = new ProfileDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = user.FullName,
                    LastLogin = user.LastLogin,
                    IsVerified = user.IsVerified,
                    Email = user.Email,
                    Followers = user.Followers,
                    Following = user.Following,
                    IsFrozen = user.IsFrozen,
                    ProfileImg = user.ProfileImg,
                    Bio = user.Bio,
                    CreatedAt = user.CreatedAt,
                }
            };
        }
    }
}
