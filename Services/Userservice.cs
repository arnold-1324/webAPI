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

            string weatherData = string.Empty;
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=53.1%2C-0.13"),
                };

                request.Headers.Add("x-rapidapi-key","c263dc9547msh6a3b8befac77f1ep1903f6jsn85c748a598a3");
                request.Headers.Add("x-rapidapi-host", "weatherapi-com.p.rapidapi.com");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Deserialize the response into a string or object
                weatherData = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Assign error message if the API call fails
                weatherData = $"Error: {ex.Message}";
            }

            // Return the UserProfileDto
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
                },
                Weather = weatherData, // Now a string
            };
        }
    }
}
