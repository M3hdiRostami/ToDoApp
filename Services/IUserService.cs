using System.Threading.Tasks;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public interface IUserService : ITodoOperatable
    {
        Task<bool> UserExists(string username);
        Task<User> CreateUser(ProfileCreateRequestCommand profileCreateRequestCommand);
        Task<User> GetUser(string username);
        Task UpdateUser(User user);
    }

}
