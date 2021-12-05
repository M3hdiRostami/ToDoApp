using System;
using System.Security.Claims;
using ToDoAPI.Models;

namespace ToDoAPI.Extensions
{

    public static class SecurityExtensions
    {
        public static User GetUserInfoFromClaims(this ClaimsPrincipal AppUser) => new User
        {
            Username = AppUser.FindFirstValue(nameof(User.Username)) ?? "unAuthorized user",
            ID = Guid.Parse(AppUser.FindFirstValue(nameof(User.ID)))

        };



    }
}
