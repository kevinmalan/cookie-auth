using API.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
