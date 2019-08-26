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
    public class QQController : ControllerBase
    {
        private readonly QQAuthRequest _authQQRequest;

        public QQController()
        {
            var authConfig = new AuthConfig("id", "key", "redirectUrl");
            _authQQRequest = new QQAuthRequest(authConfig);
        }

        /// <summary>
        /// 授权地址。
        /// </summary>
        /// <returns></returns>
        [HttpGet("Authorize")]
        public IActionResult Authorize()
        {
            var state = Guid.NewGuid().ToString();
            var url = _authQQRequest.Authorize(state);
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
            var userInfo = _authQQRequest.Login(callback);
            return Ok(userInfo);
        }
    }
}
