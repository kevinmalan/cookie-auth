using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HealthController : BaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("cookieauth-api.localhost.com is running...");
        }
    }
}
