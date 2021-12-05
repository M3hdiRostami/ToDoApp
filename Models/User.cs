
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToDoAPI.Services;

namespace ToDoAPI.Models
{
    public class User
    {
        public Guid ID { get; set; }

        [JsonPropertyName("user")]
        public string Username { get; set; }

        public List<ToDoTask> ToDos { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }


    }
}
