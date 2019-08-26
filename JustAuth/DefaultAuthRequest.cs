using JustAuth.Interfaces;
using JustAuth.Models;
using JustAuth.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace JustAuth
{
    public abstract class DefaultAuthRequest
    {
        protected readonly AuthConfig _config;
        protected readonly IAuthSource _source;

        protected DefaultAuthRequest(AuthConfig config, IAuthSource authSource)
        {
            _config = config;
            _source = authSource;
        }


        #region 公开的接口   

        /// <summary>
        /// 返回授权url。
        /// </summary>
        /// <returns>授权url。</returns>
        public string Authorize()
        {
            return Authorize(null);
        }

        /// <summary>
        /// 返回带 state 参数的授权 url，授权回调时会带上 state。
        /// </summary>
        /// <param name="state">随机字符。</param>
        /// <returns></returns>
        public string Authorize(string state)
        {
            return GetAuthorize(state);
        }

        /// <summary>
        /// 第三方登录。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AuthUser Login(AuthCallback authCallback)
        {
            var authToken = GetAccessToken(authCallback);
            return GetUserInfo(authToken);
        }

        /// <summary>
        /// 刷新 Access token。
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public string Refresh(AuthToken authToken)
        {
            return (DoPostRefreshToken(authToken))["access_token"].ToString();
        }

        /// <summary>
        /// 撤销授权。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public void Revoke(string accessToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 开发者必须实现

        /// <summary>
        /// 获取 AuthToken。
        /// </summary>
        /// <param name="authCallback">回调信息。</param>
        /// <returns>用户信息。</returns>
        protected abstract AuthToken GetAccessToken(AuthCallback authCallback);

        /// <summary>
        /// 获取用户信息。
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        protected abstract AuthUser GetUserInfo(AuthToken authToken);

        #endregion

        #region 默认实现

        /// <summary>
        /// 获取 Authorize Url。
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual string GetAuthorize(string state)
        {
            return UrlBuilder.FromBaseUrl(_source.Authorize)
              .QueryParam("response_type", "code")
              .QueryParam("client_id", _config.ClientId)
              .QueryParam("redirect_uri", _config.RedirectUri)
              .QueryParam("scope", _config.Scope)
              .QueryParam("state", state)
              .Build();
        }

        /// <summary>
        /// 获取 accessToken 的 url 以及参数。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual string GetAccessTokenUrl(AuthCallback authCallback)
        {
            return UrlBuilder.FromBaseUrl(_source.AccessToken)
                .QueryParam("grant_type", "authorization_code")
                .QueryParam("client_id", _config.ClientId)
                .QueryParam("client_secret", _config.ClientSecret)
                .QueryParam("redirect_uri", _config.RedirectUri)
                .QueryParam("code", authCallback.Code)
                .Build();
        }

        /// <summary>
        /// 获取 accessToken 请求。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual JObject DoPostAuthorizationCode(AuthCallback authCallback)
        {
            return HttpRequest.Post<JObject>(GetAccessTokenUrl(authCallback), null);
        }

        /// <summary>
        /// 获取用户信息的 url 以及参数。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected virtual string GetUserInfoUrl(AuthToken authToken)
        {
            return UrlBuilder.FromBaseUrl(_source.UserInfo)
                .QueryParam("access_token", authToken.AccessToken)
                .Build();
        }

        /// <summary>
        /// 获取用户信息请求。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected virtual JObject DoGetUserInfo(AuthToken authToken)
        {
            var url = GetUserInfoUrl(authToken);
            return HttpRequest.Get<JObject>(url);
        }

        /// <summary>
        /// 获取 refreshToken 的 url 以及参数。
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        protected virtual string GetRefreshTokenUrl(AuthToken authToken)
        {
            var url = new UriBuilder(_source.Refresh);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["refresh_token"] = authToken.RefreshToken;
            query["client_id"] = _config.ClientId;
            query["client_secret"] = _config.ClientSecret;
            query["grant_type"] = "refresh_token";
            query["redirect_uri"] = _config.RedirectUri;
            url.Query = query.ToString();
            return url.ToString();
        }

        /// <summary>
        /// 刷新 accessToken 请求。
        /// </summary>
        /// <param name="refreshToken"></param>
        protected virtual JObject DoPostRefreshToken(AuthToken authToken)
        {
            return HttpRequest.Post<JObject>(GetRefreshTokenUrl(authToken), null);
        }

        /// <summary>
        /// 获取撤销授权的 url 以及参数。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected virtual string GetRevokeUrl(AuthToken authToken)
        {
            var url = new UriBuilder(_source.Revoke);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["access_token"] = authToken.AccessToken;
            url.Query = query.ToString();
            return url.ToString();
        }

        /// <summary>
        /// 撤销授权。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        protected virtual JObject DoPostRevoke(AuthToken authToken)
        {
            return HttpRequest.Post<JObject>(GetRevokeUrl(authToken), null);
        }

        #endregion
    }
}
