using JustAuth.Interfaces;
using JustAuth.Models;
using JustAuth.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JustAuth
{
    public class GithubAuthSource : IAuthSource
    {
        public string Authorize => "https://github.com/login/oauth/authorize";

        public string AccessToken => "https://github.com/login/oauth/access_token";

        public string Refresh => throw new NotImplementedException();

        public string UserInfo => "https://api.github.com/user";

        public string Revoke => throw new NotImplementedException();
    }

    public class GithubAuthRequest : DefaultAuthRequest
    {
        public GithubAuthRequest(AuthConfig config) : base(config, new GithubAuthSource())
        {
        }

        protected override string GetAccessTokenUrl(AuthCallback authCallback)
        {
            return base.GetAccessTokenUrl(authCallback) + $"&state={authCallback.State}";
        }

        protected override AuthToken GetAccessToken(AuthCallback authCallback)
        {
            var jObject = DoPostAuthorizationCode(authCallback);

            if (jObject["error"] != null)
            {
                var error = jObject["error"].ToString();
                var errorDescription = jObject["error_description"].ToString();
                throw new AuthException(error, errorDescription);
            }

            var accessToken = jObject["access_token"].ToString();
            var scope = jObject["scope"].ToString();
            var tokenType = jObject["token_type"].ToString();
            return new AuthToken
            {
                AccessToken = accessToken,
                Scope = scope,
                TokenType = tokenType
            };
        }

        protected override AuthUser GetUserInfo(AuthToken authToken)
        {
            var jObject = DoGetUserInfo(authToken);

            return new AuthUser
            {
                AuthToken = authToken,
                Avatar = jObject["avatar_url"].ToString(),
                NickName = jObject["name"].ToString(),
                Email = jObject["email"].ToString()
            };
        }
    }
}
