using twitterclone.DTOs;
using twitterclone.Interfaces;
using twitterclone.Models;
using MongoDB.Driver;
using twitterclone.HelperClass;

namespace twitterclone.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

   private readonly IMongoCollection<User> _users;

    private readonly TokenHelper _tokenHelper;

    public AuthService(IUserRepository userRepository, IMongoDatabase database, TokenHelper tokenHelper  )
    {
         _users = database.GetCollection<User>("users");
        _userRepository = userRepository;
        _tokenHelper = tokenHelper;

    }

   public async Task<AuthResultDots> SignInAsync(UserSignInDots signInDto)
{
    
    var user = await _userRepository.GetUserByUsernameOrEmailAsync(signInDto.Username, signInDto.Email);

   
    if (user != null)
    {
        if (user.Username != null)
        {
            return new AuthResultDots { Success = false, Message = "Username already taken" };
        }
        if (user.Email != null)
        {
            return new AuthResultDots { Success = false, Message = "Email already taken" };
        }
    }

    
    if (signInDto.Password != signInDto.ConfirmPassword)
    {
        return new AuthResultDots { Success = false, Message = "Passwords do not match" };
    }

    
    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(signInDto.Password);

   
    var verificationToken = new Random().Next(100000, 999999).ToString();

    
    var newUser = new User
    {
        FullName = signInDto.FullName,
        Username = signInDto.Username,
        Email = signInDto.Email,
        Password = hashedPassword,
        VerificationToken = verificationToken,
        VerificationTokenExpiresAt = DateTime.Now.AddHours(1)
    };

    
    await CreateUserAsync(newUser);

    
    var token = _tokenHelper.GenerateToken(newUser);

   
    return new AuthResultDots { Success = true, Token = token,Message="User Created sucesssully" };
}



    private  async Task CreateUserAsync(User user){
        await _users.InsertOneAsync(user);
    }

}
