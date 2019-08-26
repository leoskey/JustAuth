using System;
using System.Collections.Generic;
using System.Text;

namespace JustAuth.Models
{
    public class AuthCallback
    {
        public string Code { get; set; }

        public string State { get; set; }
    }
}
