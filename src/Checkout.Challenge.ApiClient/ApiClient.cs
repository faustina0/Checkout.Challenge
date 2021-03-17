using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.Challenge.ApiClient.Config;
using Checkout.Challenge.ApiClient.Dto;
using Newtonsoft.Json;

namespace Checkout.Challenge.ApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly IApiClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClient(IApiClientSettings settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<PaymentResponse> Create(PaymentRequest payment)
        {
            var json = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(payment));
            var request = new HttpRequestMessage
                          {
                              RequestUri = BuildUri("Payment"),
                              Method = HttpMethod.Post,
                              Content = new StringContent(json, Encoding.UTF8, "application/json")
                          };

            return await Execute(request);
        }


        public async Task<PaymentResponse> Get(string id)
        {
            
            var request = new HttpRequestMessage
                          {
                              RequestUri = BuildUri($"Payment/{id}"),
                              Method = HttpMethod.Get,
                          };

            return await Execute(request);
        }



        private Uri BuildUri(string url)
        {
            if (string.IsNullOrEmpty(_settings.BaseUrl))
            {
                return new Uri(url);
            }

            if (url.StartsWith("/"))
            {
                url = url.Substring(1, url.Length - 1);
            }

            var slash = _settings.BaseUrl.EndsWith("/") ? "" : "/";

            return new Uri($"{_settings.BaseUrl}{slash}{url}");
        }

        private HttpClient GetClient()
        {
            var client = _httpClientFactory.CreateClient("DataScienceApiClient");

            return client;
        }

        private async Task<PaymentResponse> Execute(HttpRequestMessage request)
        {
            HttpResponseMessage response;

            try
            {
                response = await GetClient().SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApiClientException(e.Message);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiClientException(response);
            }

           

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PaymentResponse>(result);
        }


    }
}
