using ToDoAPI.Models;

namespace ToDoAPI.Uilities.Security.Token
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(User user);

    }
}
