using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoAPI.Models
{
    public class LoginRequestCommand
    {
        public string Username { get; set; }

        public string Password { get; set; }

    }
}
