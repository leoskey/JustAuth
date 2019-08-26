using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustAuth;
using JustAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly GithubAuthRequest _request;

        public GitHubController()
        {
            var authConfig = new AuthConfig("id", "key", "redirectUrl");
            _request = new GithubAuthRequest(authConfig);
        }

        /// <summary>
        /// 授权地址。
        /// </summary>
        /// <returns></returns>
        [HttpGet("Authorize")]
        public IActionResult Authorize()
        {
            var state = Guid.NewGuid().ToString();
            var url = _request.Authorize(state);
            return Redirect(url);
        }

        /// <summary>
        /// 授权回调。
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [HttpGet("CallBack")]
        public IActionResult CallBack([FromQuery] AuthCallback callback)
        {
            var userInfo = _request.Login(callback);
            return Ok(userInfo);
        }
    }
}
