using System;
using NotificationCenter.Model.ValueObjects;

namespace NotificationCenter.Business.Services
{
    public interface IAccessTokenGenerator
    {
        AccessToken Generate(Username username);
    }

    public class AccessToken
    {
        public string Value { get; private set; }
        public string Username { get; private set; }
        public DateTime ExpireAt { get; private set; }

        public AccessToken(string value, string username, DateTime expireAt)
        {
            Value = value;
            Username = username;
            ExpireAt = expireAt;
        }
    }
}