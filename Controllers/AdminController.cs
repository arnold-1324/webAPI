using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace twitterclone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public string getAdmin()
        {
            return "Admin";
        }
    }
}
