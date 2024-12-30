using System;
using twitterclone.DTOs;

namespace twitterclone.Interfaces;

public interface IUserService
{
   Task<UserProfileDto> GetUserProfileDtoAsync( UserName username);
}
