using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Services;
using ToDoAPI.Uilities.Responses;

namespace ToDoAPI.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : AuthAPIControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpGet]
        public ActionResult IsAuthenticated()
        {
            return Ok(User.Identity.IsAuthenticated);
        }


        [HttpGet]
        public ActionResult AuthenticatedUser()
        {

            return Ok(CurrentUser.Username);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(ProfileCreateRequestCommand profileCreateRequestCommand)
        {

            BaseResponse<User> response = await _authenticationService.SignUp(profileCreateRequestCommand);


            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }


        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginRequestCommand loginRequestCommand)
        {

            var response = await _authenticationService.SignIn(loginRequestCommand);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }


        }




    }
}