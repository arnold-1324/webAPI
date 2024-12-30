using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using twitterclone.DTOs;
using twitterclone.Interfaces;


namespace twitterclone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

           private readonly IUserService _userService;

           public UserController(IUserService userService)
           {
              _userService = userService;
           }

       [Authorize]
      [HttpGet("getUserProfile")]
      public async Task<IActionResult> GetUserProfileAsync( [FromQuery] string username)
      {
        try{
         if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _userService.GetUserProfileDtoAsync(username);

                 if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                
            
                return StatusCode(500, "An error occurred while processing your request."+ex);
            }
      }

    }
}
