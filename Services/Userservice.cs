using System;
using twitterclone.DTOs;
using twitterclone.Interfaces;
using twitterclone.Models;
using MongoDB.Driver;
namespace twitterclone.Services;

public class Userservice:IUserService
{

private readonly  IUserRepository _userRepository;
public Userservice(IUserRepository userRepository)
{
    _userRepository=userRepository;

}

public async Task<UserProfileDto> GetUserProfileDtoAsync(string  Username){
    var user = await _userRepository.GetUserByUsername(Username);
    if (user==null)
        return new UserProfileDto { Success = false, Message = "User Not Found" };
        

  var userProfileDto = new ProfileDto{
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

};

return new UserProfileDto{ Success=true,  profile=userProfileDto};
    
}

}