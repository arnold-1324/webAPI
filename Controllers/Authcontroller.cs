using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twitterclone.DTOs;
using twitterclone.Interfaces;

namespace twitterclone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authcontroller : ControllerBase
    {
         private readonly IAuthService _authService;

    public Authcontroller(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] UserSignInDots signInDto)
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
    }
}
