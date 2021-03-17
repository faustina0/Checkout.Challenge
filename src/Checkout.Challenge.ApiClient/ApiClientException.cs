using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Challenge.ApiClient
{
    public class ApiClientException:Exception
    {
        public HttpStatusCode Code { get; set; }
        public ApiClientException(string message)
            : base(message)
        {
        }

        public ApiClientException(HttpResponseMessage response)
            : base(GetResponse(response))
        {
            Code = response.StatusCode;
        }

        public static string GetResponse(HttpResponseMessage response)
        {
            return response.Content != null ? (Task.Run(() => response.Content.ReadAsStringAsync()).Result) : "<none>";
        }
    }
}
