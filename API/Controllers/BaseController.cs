using API.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
