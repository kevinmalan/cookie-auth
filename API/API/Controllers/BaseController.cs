using API.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiException]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
