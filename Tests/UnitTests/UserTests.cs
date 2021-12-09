using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Services;
using ToDoAPI.Tests.Helpers;
using ToDoAPI.Uilities.Responses;
using ToDoAPI.Uilities.Security.Token;
using Xunit;

namespace ToDoAPI.Tests.UnitTests
{
    public class UserTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {

        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        public UserTests(CustomWebApplicationFactory<Startup> factory)
        {
            _userService = factory.GetService<IUserService>();
            _authenticationService = factory.GetService<IAuthenticationService>();

        }


        [Fact]
        public async Task<User> SignUpUserAsync()
        {

            //create user
            var userProfile = GetUserProfileCreateRequestCommand();
            BaseResponse<User> response = await _authenticationService.SignUp(userProfile);

            if (response.Success)
            {
                Assert.Equal(userProfile.UserName, response.Data.Username);
                Assert.Equal(userProfile.Email, response.Data.Email);
                Assert.Equal(userProfile.FirstName, response.Data.FirstName);
                Assert.Equal(userProfile.LastName, response.Data.LastName);
                return response.Data;
            }
            else
            {
                Assert.True(response.Message.Contains("already exists"));
                return userProfile.GetUser() with { Password = userProfile.Password };
            }

        }


        [Fact]
        public async Task<AccessToken> SignInUserAsync()
        {
            var userProfile = await SignUpUserAsync();

            var response = await _authenticationService.SignIn(new LoginRequestCommand { Password = userProfile.Password, Username = userProfile.Username });

            Assert.Equal(response.Success, true);
            Assert.NotNull(response.Data);

            return response.Data;

        }


        [Fact]
        public async Task<ICollection<ToDoTask>> GetUserToDoListAsync()
        {
            var userProfile = await SignUpUserAsync();

            var response = await _userService.GetUserToDoList(userProfile.Username);

            Assert.Equal(response.Success, true);
            Assert.NotNull(response.Data);

            return response.Data;

        }

        [Fact]
        public async Task UpdateToDoAsync()
        {
            var task = await AddToDoAsync();
            var newTask = task with { Status = "Updated" };
            var userProfile = GetUserProfileCreateRequestCommand();

            var response = await _userService.UpdateToDo(userProfile.UserName, newTask);

            Assert.NotNull(response.Data);
            Assert.Equal(response.Data, newTask);
            Assert.Equal(response.Success, true);


        }


        [Fact]
        public async Task DeleteToDoAsync()
        {
            var newTask = await AddToDoAsync();
            var userProfile = GetUserProfileCreateRequestCommand();

            var response = await _userService.DeleteToDo(userProfile.UserName, newTask.ID);

            Assert.Equal(response.Success, true);
            Assert.NotNull(response.Data);


        }


        [Fact]
        public async Task<ToDoTask> AddToDoAsync()
        {
            var userProfile = await SignUpUserAsync();

            ToDoTask newTask = new ToDoTask
            {
                Description = "Meet with manager",
                DueTime = Convert.ToDateTime("2021-12-09T13:14:39.513Z"),
                Status = "pending",
                Tags = new List<string> { "Work", "crucial" }
            };
            var response = await _userService.AddToDo(userProfile.Username, new List<ToDoTask> { newTask });

            Assert.Equal(response.Success, true);
            Assert.NotNull(response.Data);

            //created task 
            return response.Data.FirstOrDefault();

        }

        private ProfileCreateRequestCommand GetUserProfileCreateRequestCommand()
        {
            return new ProfileCreateRequestCommand()
            {
                UserName = "M.Rostami",
                FirstName = "Mahdi",
                LastName = "Rostami",
                Email = "MahdiRostami@outlook.de",
                Password = "password"
            };
        }

    }
}
