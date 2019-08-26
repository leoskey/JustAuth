using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace JustAuth.Models
{
    public class AuthUser
    {
        public AuthToken AuthToken { get; set; }
        public string NickName { get; internal set; }
        public string Avatar { get; internal set; }
        public string Email { get; internal set; }
    }
}
