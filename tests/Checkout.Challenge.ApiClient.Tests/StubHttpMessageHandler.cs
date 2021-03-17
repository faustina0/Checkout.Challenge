
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Challenge.ApiClient.Tests
{
    internal class StubHttpMessageHandler : HttpMessageHandler
    {
        private HttpRequestMessage lastRequest;
        private HttpResponseMessage lastMessage;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            lastRequest = request;
            return Task.FromResult(lastMessage);
        }

        public bool AssertUrl(string url)
        {
            return lastRequest.RequestUri.ToString() == url;
        }

        public bool AssertHttpMethod(HttpMethod method)
        {
            return lastRequest.Method == method;
        }

        public void SetResponse(string message, HttpStatusCode statusCode)
        {
            lastMessage = new HttpResponseMessage
                          {
                              StatusCode = statusCode,
                              Content = new StringContent(message)
                          };
            lastMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        }


    }
}