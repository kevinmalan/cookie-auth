using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SecretController : BaseController
    {
        [Authorize]
        [HttpGet]
        [Route("secret")]
        public IActionResult Get()
        {
            return Ok("bazinga");
        }
    }
}
