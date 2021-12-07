using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Uilities.Responses;
using ToDoAPI.Uilities.Security.Token;

namespace ToDoAPI.Services
{
    public interface IAuthenticationService
    {
        Task<BaseResponse<User>> SignUp(ProfileCreateRequestCommand profileCreateRequestCommand);

        Task<BaseResponse<AccessToken>> SignIn(LoginRequestCommand loginCommmand);

    }
}
