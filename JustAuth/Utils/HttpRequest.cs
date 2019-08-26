using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JustAuth.Utils
{
    public static class HttpRequest
    {
        private static readonly IRestClient _client = new RestClient()
             .UseNewtonsoftJson();

        public static string Get(string url)
        {
            var request = new RestRequest(url);
            var response = _client.Get(request);
            return response.Content;
        }

        public static TResult Get<TResult>(string url)
        {
            var request = new RestRequest(url);
            var response = _client.Get<TResult>(request);
            return response.Data;
        }

        public static TResult Post<TResult>(string url, object body = null)
        {
            var request = new RestRequest(url);
            if (!(body is null))
            {
                request.AddJsonBody(body);
            }
            var response = _client.Post<TResult>(request);
            return response.Data;
        }
    }
}
