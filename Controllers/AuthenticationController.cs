using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Services;
using ToDoAPI.Uilities.Responses;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService service)
        {
            this.authenticationService = service;
        }


        [HttpGet]
        public ActionResult IsAuthenticated()
        {
            return Ok(User.Identity.IsAuthenticated);
        }
        [HttpGet]
        public ActionResult AuthenticatedUser()
        {

            return Ok(User.Identity.Name);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(ProfileCreateRequestCommand profileCreateRequestCommand)
        {

            BaseResponse<User> response = await authenticationService.SignUp(profileCreateRequestCommand);


            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }


        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginCommmand signInViewModel)
        {

            var response = await authenticationService.SignIn(signInViewModel);

            if (response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }


        }




    }
}