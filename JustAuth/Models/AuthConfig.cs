using System;
using System.Collections.Generic;
using System.Text;

namespace JustAuth
{
    public class AuthConfig
    {
        public AuthConfig(string clientId, string clientSecret, string redirectUri)
        {
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            RedirectUri = redirectUri ?? throw new ArgumentNullException(nameof(redirectUri));
        }

        public AuthConfig(string clientId, string clientSecret, string redirectUri, string scope) : this(clientId, clientSecret, redirectUri)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
    }
}
