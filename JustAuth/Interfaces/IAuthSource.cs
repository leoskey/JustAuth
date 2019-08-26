using System;
using System.Collections.Generic;
using System.Text;

namespace JustAuth.Interfaces
{
    /// <summary>
    /// 接口地址。
    /// </summary>
    public interface IAuthSource
    {
        /// <summary>
        /// Authorization Code 的 Url。
        /// </summary>
        string Authorize { get; }
        /// <summary>
        /// Access Token 的 Url。
        /// </summary>
        string AccessToken { get; }
        /// <summary>
        /// 权限自动续期的 Url。
        /// </summary>
        string Refresh { get; }
        /// <summary>
        /// 用户信息的 Url。
        /// </summary>
        string UserInfo { get; }
        /// <summary>
        /// 撤销授权的 Url。
        /// </summary>
        string Revoke { get; }
    }
}
