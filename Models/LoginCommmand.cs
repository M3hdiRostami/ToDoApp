using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoAPI.Models
{
    public class LoginCommmand
    {
        [JsonPropertyName("user")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

    }
}
