
using System;
using System.Collections.Generic;

namespace ToDoAPI.Models
{
    public record User
    {
        public User()
        {
            ToDos = new List<ToDoTask>();
        }
        public Guid ID { get; set; }

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
