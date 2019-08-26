using System;
using System.Collections.Generic;
using System.Text;

namespace JustAuth.Models
{
    public class AuthToken
    {
        public AuthToken()
        {
        }

        public AuthToken(string accessToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }

        public AuthToken(string accessToken, int expireIn)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            ExpireIn = expireIn;
        }

        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
        public string RefreshToken { get; set; }
        public string OpenId { get; set; }
        public string UnionId { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
    }
}
