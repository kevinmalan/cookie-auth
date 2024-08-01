using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

namespace API.Controllers
{
    public class HealthController(ILogger<HealthController> logger) : BaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            logger.LogInformation("CookieAuth API is running...");
            return Ok("cookieauth-api.localhost.com is running...");
        }

        [HttpGet]
        [Route("GetDefaultException")]
        public IActionResult GetDefaultException()
        {
            throw new Exception("Sample default exception message");
        }

        [HttpGet]
        [Route("GetCustomException")]
        public IActionResult GetCustomException()
        {
            throw new BadRequestException("Sample custom exception message with CustomData", new { Id = 15, Name = "BadRequestException" });
        }
    }
}
