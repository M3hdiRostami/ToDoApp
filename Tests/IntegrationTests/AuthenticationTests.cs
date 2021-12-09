using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Tests.Helpers;
using ToDoAPI.Uilities.Responses;
using ToDoAPI.Uilities.Security.Token;
using Xunit;

namespace ToDoAPI.Tests.IntegrationTests
{
    public class AuthenticationTests
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly string _baseHostname = "/api/Authentication";
        public AuthenticationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();

        }


        [Fact]
        public async Task IsAuthenticatedUser()
        {
            var getResponse = await _client.GetAsync($"{_baseHostname}/IsAuthenticated");
            var getJsonResult = await getResponse.Content.ReadAsStringAsync();

            bool IsAuthenticated = Convert.ToBoolean(getJsonResult);

            Assert.Equal(IsAuthenticated, false);
        }

        [Fact]
        public async Task ShouldReturnAuthorizedUserName()
        {
            await SignInUserAsync();

            var getResponse = await _client.GetAsync($"{_baseHostname}/AuthenticatedUser");
            var jsonResults = await getResponse.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(jsonResults, GetUserProfileCreateRequestCommand().UserName);

        }


        [Fact]
        public async Task<User> SignUpUserAsync()
        {

            //create user
            var userProfile = GetUserProfileCreateRequestCommand();
            var newUser = JsonConvert.SerializeObject(userProfile);
            var content = new StringContent(newUser, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_baseHostname}/SignUp", content);

            var jsonResults = await response.Content.ReadAsStringAsync();
            var newUserBaseResponse = JsonConvert.DeserializeObject<BaseResponse<User>>(jsonResults);

            if (newUserBaseResponse.Success)
            {
                Assert.Equal(userProfile.UserName, newUserBaseResponse.Data.Username);
                Assert.Equal(userProfile.Email, newUserBaseResponse.Data.Email);
                Assert.Equal(userProfile.FirstName, newUserBaseResponse.Data.FirstName);
                Assert.Equal(userProfile.LastName, newUserBaseResponse.Data.LastName);
                return newUserBaseResponse.Data;
            }
            else
            {
                Assert.True(newUserBaseResponse.Message.Contains("already exists"));
                return userProfile.GetUser() with { Password = userProfile.Password };
            }

        }


        [Fact]
        public async Task<AccessToken> SignInUserAsync()
        {
            var userProfile = await SignUpUserAsync();

            var newUser = JsonConvert.SerializeObject(new LoginRequestCommand { Password = userProfile.Password,Username = userProfile.Username });

            var content = new StringContent(newUser, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_baseHostname}/SignIn", content);

            var jsonResults = await response.Content.ReadAsStringAsync();
            var newTokenBaseResponse = JsonConvert.DeserializeObject<BaseResponse<AccessToken>>(jsonResults);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(newTokenBaseResponse.Data);

            //save access token for next requests
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokenBaseResponse.Data.Token);

            return newTokenBaseResponse.Data;

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
