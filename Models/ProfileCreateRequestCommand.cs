using System;

namespace ToDoAPI.Models
{
    public class ProfileCreateRequestCommand
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User GetUser()
        {
            return new User
            {
                ID = Guid.NewGuid(),
                Username = UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(this.Password)
            };
        }
    }
}
