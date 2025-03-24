using twitterclone.DTOs;
using twitterclone.Interfaces;
using twitterclone.Models;

using twitterclone.HelperClass;



namespace twitterclone.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
   
    private readonly TokenHelper _tokenHelper;

   // private readonly EmailSender _emailSender;
   // private readonly EmailTemplate _emailTemplate;

    
    public AuthService(IUserRepository userRepository,EmailTemplate emailTemplate, TokenHelper tokenHelper, EmailSender emailSender)
    {
       //  _users = database.GetCollection<User>("users");
        _userRepository = userRepository;
        _tokenHelper = tokenHelper;
      // _emailSender = emailSender; 
       // _emailTemplate=emailTemplate;
       

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


        // var body = _emailTemplate.VerificationEmailTemplate.Replace("{verificationCode}", verificationToken);

        // var emailDto = new EmailDto
        // {
        //     ToEmail = newUser.Email,
        //     Subject = "Verify Your Email",
        //     Body = body
        // };

       // await _emailSender.SendEmailAsync(emailDto);



    await _userRepository.CreateUserAsync(newUser);
    var token =  _tokenHelper.GenerateToken(newUser);



    var userDTO = new UserDTO
{
    Id = newUser.Id,  
    Username = newUser.Username,
    FullName = newUser.FullName,
    LastLogin = newUser.LastLogin,
    IsVerified = newUser.IsVerified,
    Email = newUser.Email,
    Followers = newUser.Followers,
    Following = newUser.Following,
    IsFrozen = newUser.IsFrozen,
    ProfileImg = newUser.ProfileImg,
    Bio = newUser.Bio,
    CreatedAt = newUser.CreatedAt,
    UpdatedAt = newUser.UpdatedAt
};


    
    
  

   
    return new AuthResultDots { Success = true, Token = token,Message="User Created sucesssully",User=userDTO };
}

   public async Task<AuthResultDots> LoginAsync(LoginDots loginDto){
    var user = await _userRepository.GetUserByUsername(loginDto.Username);

    if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password)){
        return new AuthResultDots { Success = false, Message = "Invalid User credentials" };
    }
   
   var token =  _tokenHelper.GenerateToken(user);

   var userDTO = new  UserDTO{
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
    UpdatedAt = user.UpdatedAt
   };

   return new AuthResultDots{ Success=true, Token=token, Message="User Logged In successfully",User=userDTO};

}


   
 public async Task<AuthResultDots> LogoutAsync(string token)
{
    
    var isTokenRevoked = await _tokenHelper.RevokeTokenAsync(token);

    if (!isTokenRevoked)
    {
        return new AuthResultDots { Success = false, Message = "Logout failed. Token could not be revoked." };
    }

    
    return new AuthResultDots { Success = true, Message = "Logout successful." };
}


    

}
