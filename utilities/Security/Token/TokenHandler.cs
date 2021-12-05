using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoAPI.Models;

namespace ToDoAPI.Uilities.Security.Token
{
    public class TokenHandler : ITokenHandler
    {

        private readonly CustomTokenOptions tokenOptions;

        public TokenHandler(IOptions<CustomTokenOptions> tokenOptions)
        {
            this.tokenOptions = tokenOptions.Value;
        }
        public AccessToken CreateAccessToken(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(tokenOptions.AccessTokenExpiration);

            var securityKey = SignHandler.GetSecurityKey(tokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaim(user),
                signingCredentials: signingCredentials

                );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            AccessToken accessToken = new AccessToken();

            accessToken.Token = token;
            accessToken.Expiration = accessTokenExpiration;

            return accessToken;
        }


        private IEnumerable<Claim> GetClaim(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(nameof(user.ID),$"{user.ID}"),
                new Claim(nameof(user.Username),$"{user.Username}")
            };

            return claims;
        }
    }
}
