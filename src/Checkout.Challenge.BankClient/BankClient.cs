using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Challenge.BankClient.Config;
using Checkout.Challenge.BankClient.Dto;
using Newtonsoft.Json;

namespace Checkout.Challenge.BankClient
{
    public class BankClient : IBankClient
    {
        private readonly IBankClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public BankClient(IBankClientSettings settings, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

       
       
        public async Task<BankPaymentResponse> RequestPayment(BankPaymentRequest payment)
        {
            var json = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(payment));
            var request = new HttpRequestMessage
                          {
                              RequestUri = BuildUri("Payment"),
                              Method = HttpMethod.Post,
                              Content = new StringContent(json, Encoding.UTF8, "application/json")
                          };

            HttpResponseMessage response;

            try
            {
                response = await GetClient().SendAsync(request);
            }
            catch (Exception e)
            {
                throw new BankClientException(e.Message);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new BankClientException(response);
            }

            var result = await response.Content.ReadAsStringAsync();
            var bankResponse = JsonConvert.DeserializeObject<PaymentResponse>(result);

            var clientResponse = _mapper.Map<PaymentResponse, BankPaymentResponse>(bankResponse);
            clientResponse.Approved = bankResponse.StatusCode == "10000";

            return clientResponse;
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

    }
}
