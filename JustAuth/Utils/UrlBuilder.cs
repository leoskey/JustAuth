using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace JustAuth.Utils
{
    public class UrlBuilder
    {
        private readonly UriBuilder uriBudiler;

        public UrlBuilder(string uri)
        {
            uriBudiler = new UriBuilder(uri);
        }

        public static UrlBuilder FromBaseUrl(string url)
        {
            var urlBuilder = new UrlBuilder(url);
            return urlBuilder;
        }

        public UrlBuilder QueryParam(string key, string value)
        {
            var query = HttpUtility.ParseQueryString(uriBudiler.Query);
            query.Add(key, value);
            uriBudiler.Query = query.ToString();
            return this;
        }

        public string Build()
        {
            if (uriBudiler.Port == 80 || uriBudiler.Port == 443)
            {
                uriBudiler.Port = -1;
            }
            return uriBudiler.ToString();
        }
    }
}
