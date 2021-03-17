using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Challenge.BankClient
{
    public class BankClientException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public BankClientException(string message)
            : base(message)
        {
        }

        public BankClientException(HttpResponseMessage response)
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
