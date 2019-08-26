using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Xunit;

namespace Tests.Request
{
    public class AuthQQRequest
    {
        [Theory]
        [InlineData("abc", "abc", "http://localhost", "get_user_info")]
        [InlineData("abcd", "abcd", "http://localhost", "get_user_info")]
        public void GetAuthorize_ReturnUrl(string clientId, string clientSecret, string redirectUrl, string scope)
        {
            var config = new JustAuth.AuthConfig(clientId, clientSecret, redirectUrl, scope);
            var request = new JustAuth.QQAuthRequest(config);

            var url = request.Authorize();
            var expected = $"https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={clientId}&redirect_uri={HttpUtility.UrlEncode(redirectUrl)}";

            Assert.StartsWith(expected, url);
        }

        [Theory]
        [InlineData("abc", "abc", "http://localhost", "get_user_info")]
        [InlineData("abcd", "abcd", "http://localhost", "get_user_info")]
        public void GetAuthorizeWithState_ReturnUrl(string clientId, string clientSecret, string redirectUrl, string scope)
        {
            var state = Guid.NewGuid().ToString();

            var config = new JustAuth.AuthConfig(clientId, clientSecret, redirectUrl, scope);
            var request = new JustAuth.QQAuthRequest(config);

            var url = request.Authorize(state);
            var expected = $"https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={clientId}&redirect_uri={HttpUtility.UrlEncode(redirectUrl)}&scope={scope}&state={state}";

            Assert.Equal(expected, url);
        }
    }
}
