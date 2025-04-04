using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twitterclone.DTOs;
using twitterclone.Interfaces; 

namespace twitterclone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDots signInDto) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.SignInAsync(signInDto);

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

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDots loginDto)
        {
           try{
             if(!ModelState.IsValid){
                 return BadRequest(ModelState);
             }
             var result = await _authService.LoginAsync(loginDto);
             if(!result.Success){
                 return Unauthorized(result.Message);
             }

             return Ok(result);
           }catch (Exception ex){
               return StatusCode(500, "An error occurred while processing your request."+ex);
           }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromHeader] string token){
             if(string.IsNullOrEmpty(token)){
                 return BadRequest(new { Message = "Token is required" });
             }

             var result =await _authService.LogoutAsync(token);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }
            return Ok(new { Message = "User logged out successfully." });
        }
        
    }
}
 
       
