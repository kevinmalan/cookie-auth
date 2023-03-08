using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Requests;

namespace API.Controllers
{
    public class ProfileController : BaseController
    {
        [AllowAnonymous]
        public async Task Register([FromBody] RegisterProfileRequest request)
        {

        }
    }
}
