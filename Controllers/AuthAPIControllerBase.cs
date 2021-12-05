using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Extensions;

namespace API.Controllers
{


    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthAPIControllerBase : ControllerBase
    {

        public ToDoAPI.Models.User CurrentUser => User.GetUserInfoFromClaims();
        public AuthAPIControllerBase()
        {

        }

    }
}
