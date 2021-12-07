using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Uilities.Responses;
using ToDoAPI.Uilities.Security.Token;

namespace ToDoAPI.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenHandler tokenHandler;
        private readonly CustomTokenOptions tokenOptions;
        private readonly IUserService userService;



        public AuthenticationService(IUserService userService, ITokenHandler tokenHandler, IOptions<CustomTokenOptions> tokenOptions)
        {
            this.tokenHandler = tokenHandler;
            this.userService = userService;
            this.tokenOptions = tokenOptions.Value;

        }

        public async Task<BaseResponse<AccessToken>> SignIn(LoginRequestCommand loginCommmand)
        {

            User user = await userService.GetUser(loginCommmand.Username);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(loginCommmand.Password, user.Password))
                {
                    AccessToken accessToken = tokenHandler.CreateAccessToken(user);
                    return new BaseResponse<AccessToken>(accessToken);
                }


            }

            return new BaseResponse<AccessToken>("username or password doesn't match!");

        }

        public async Task<BaseResponse<User>> SignUp(ProfileCreateRequestCommand profileCreateRequestCommand)
        {

            if (await userService.UserExists(profileCreateRequestCommand.UserName))
            {
                return new BaseResponse<User>($"Username '{profileCreateRequestCommand.UserName}' already exists");
            }
            User user = await userService.CreateUser(profileCreateRequestCommand);
            return new BaseResponse<User>(user);

        }

    }
}
