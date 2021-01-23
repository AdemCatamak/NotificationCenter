using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Business.Services.Imp
{
    public class JwtAccessTokenGenerator : IAccessTokenGenerator
    {
        public const string JWT_KEY = "NotificationCenter_JwtKey";
        public const string JWT_ISSUER = "NotificationCenter_JwtIssuer";
        public const string JWT_AUDIENCE = "NotificationCenter_JwtAudience";

        public AccessToken Generate(Username username)
        {
            DateTime expireAt = DateTime.UtcNow.AddDays(1);

            Claim[] claims = {new Claim(ClaimTypes.NameIdentifier, username.ToString())};

            string tokenValue = GenerateJwtToken(expireAt, claims);

            return new AccessToken(tokenValue, username.ToString(), expireAt);
        }

        private string GenerateJwtToken(DateTime expireTime, IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_KEY));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(JWT_ISSUER,
                                             JWT_AUDIENCE,
                                             claims,
                                             expires: expireTime,
                                             signingCredentials: credentials);


            string jwtTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtTokenValue;
        }
    }
}