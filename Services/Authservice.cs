using twitterclone.DTOs;
using twitterclone.Interfaces;
using twitterclone.Models;
using MongoDB.Driver;
using System.Net;
using System.Net.Mail;
using twitterclone.HelperClass;
using Microsoft.Extensions.Configuration;


namespace twitterclone.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
   private readonly IConfiguration _configuration;
   private readonly IMongoCollection<User> _users;
    private readonly TokenHelper _tokenHelper;

    private readonly EmailTemplate _emailTemplate;
    public AuthService(IUserRepository userRepository,IConfiguration configuration ,EmailTemplate emailTemplate, IMongoDatabase database, TokenHelper tokenHelper)
    {
         _users = database.GetCollection<User>("users");
        _userRepository = userRepository;
        _configuration=configuration;
        _tokenHelper = tokenHelper;
        _emailTemplate=emailTemplate;
       

    }

public async Task SendEmailAsync(EmailDto emailDto)
{

 var smtpClient = new SmtpClient
        {
            Host = _configuration["SmtpSettings:Server"],
            Port = int.Parse(_configuration["SmtpSettings:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _configuration["SmtpSettings:Username"],
                _configuration["SmtpSettings:Password"]
            )
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpSettings:SenderEmail"], _configuration["SmtpSettings:SenderName"]),
            Subject = emailDto.Subject,
            Body = emailDto.Body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(emailDto.ToEmail);

        await smtpClient.SendMailAsync(mailMessage);

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


var body = _emailTemplate.VerificationEmailTemplate.Replace("{verificationCode}", verificationToken);

        var emailDto = new EmailDto
        {
            ToEmail = newUser.Email,
            Subject = "Verify Your Email",
            Body = body
        };

        await SendEmailAsync(emailDto);



    await CreateUserAsync(newUser);
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




    private  async Task CreateUserAsync(User user){
        await _users.InsertOneAsync(user);
    }

}
