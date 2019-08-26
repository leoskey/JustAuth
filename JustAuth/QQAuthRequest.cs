using JustAuth.Interfaces;
using JustAuth.Models;
using JustAuth.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JustAuth
{
    internal class QQAuthSource : IAuthSource
    {
        public string Authorize => "https://graph.qq.com/oauth2.0/authorize";

        public string AccessToken => "https://graph.qq.com/oauth2.0/token";

        public string Refresh => "https://graph.qq.com/oauth2.0/token";

        public string UserInfo => "https://graph.qq.com/user/get_user_info";

        public string Revoke => throw new NotImplementedException();
    }

    public class QQAuthRequest : DefaultAuthRequest
    {
        public QQAuthRequest(AuthConfig config) : base(config, new QQAuthSource())
        {
        }

        protected override AuthToken GetAccessToken(AuthCallback authCallback)
        {
            var url = GetAccessTokenUrl(authCallback);
            var stringResult = HttpRequest.Get(url);

            var accessTokenResult = ResultParse<JObject>(stringResult);

            if (accessTokenResult != null)
            {
                var error = accessTokenResult["error"].ToString();
                var errorMessage = accessTokenResult["error_description"].ToString();
                throw new AuthException($"{error}:{errorMessage}");
            }

            var queryString = HttpUtility.ParseQueryString(stringResult);
            var token = new AuthToken
            {
                AccessToken = queryString["access_token"],
                ExpireIn = int.Parse(queryString["expires_in"]),
                RefreshToken = queryString["refresh_token"]
            };
            return token;
        }

        protected override AuthUser GetUserInfo(AuthToken authToken)
        {
            // 获取 OpenId
            string openIdUrl = GetOpenIdUrl(authToken.AccessToken);
            var openIdString = HttpRequest.Get(openIdUrl);
            var openIdResult = ResultParse<JObject>(openIdString);
            authToken.OpenId = openIdResult["openid"].ToString();

            // 获取用户信息
            var userInfoUrl = GetUserInfoUrl(authToken);
            var userInfoString = HttpRequest.Get(userInfoUrl);
            var userInfoReslt = ResultParse<JObject>(userInfoString);

            var avatar = userInfoReslt["figureurl_qq_2"].ToString();
            if (string.IsNullOrWhiteSpace(avatar))
            {
                avatar = userInfoReslt["figureurl_qq_1"].ToString();
            }

            return new AuthUser
            {
                AuthToken = authToken,
                NickName = userInfoReslt["nickname"].ToString(),
                Avatar = avatar,

            };
        }

        protected override string GetUserInfoUrl(AuthToken authToken)
        {
            var url = base.GetUserInfoUrl(authToken);
            return UrlBuilder.FromBaseUrl(url)
                .QueryParam("oauth_consumer_key", _config.ClientId)
                .QueryParam("openid", authToken.OpenId)
                .Build();
        }

        /// <summary>
        /// 获取 OpenId Url。
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        private string GetOpenIdUrl(string accessToken)
        {
            return UrlBuilder.FromBaseUrl("https://graph.qq.com/oauth2.0/me")
                .QueryParam("access_token", accessToken)
                .Build();
        }

        /// <summary>
        /// 格式化返回结果。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        private T ResultParse<T>(string content)
        {
            content = content
                  .Replace("callback(", string.Empty)
                  .Replace(")", string.Empty)
                  .Replace(";", string.Empty)
                  .Trim();
            if (content.StartsWith("{"))
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            return default;
        }
    }
}
