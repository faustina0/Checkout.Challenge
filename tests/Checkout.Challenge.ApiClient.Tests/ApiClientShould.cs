using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.Challenge.ApiClient.Config;
using Checkout.Challenge.ApiClient.Dto;
using FizzWare.NBuilder;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Checkout.Challenge.ApiClient.Tests
{
    [TestFixture]
    public class ApiClientShould
    {
        [SetUp]
        public void SetUp()
        {
            _settings = new ApiClientSettings
                        {
                            BaseUrl = "http://someurl.com"
            };
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _httpMessageHandler = new StubHttpMessageHandler();
            _httpClientFactory.CreateClient(Arg.Any<string>())
                              .Returns(new HttpClient(_httpMessageHandler));
            _target = new ApiClient(_settings, _httpClientFactory);
            
        }

        [Test]
        public async Task ReturnTheCorrectResultOnGet()
        {
            await AssumeApiResponseInitialized(HttpStatusCode.OK);
            var response = await _target.Get("4545454");
            AssertResult("Payment/4545454", HttpMethod.Get, response);
        }
        
        [Test]
        public void ReturnErrorWhenHttpStatusCodeNotOKOnGet()
        {
            _httpMessageHandler.SetResponse("", HttpStatusCode.InternalServerError);
            Assert.CatchAsync<ApiClientException>(async () => await _target.Get("4545454"));
        }


        [Test]
        public async Task ReturnTheCorrectResultOnPost()
        {
            await AssumeApiResponseInitialized(HttpStatusCode.OK);
            var paymentRequest = Builder<PaymentRequest>.CreateNew()
                                                        .Build();
            var response = await _target.Create(paymentRequest);
            AssertResult("Payment", HttpMethod.Post, response);
        }


        [Test]
        public void ReturnErrorWhenHttpStatusCodeNotOKOnCreate()
        {
            var paymentRequest = Builder<PaymentRequest>.CreateNew()
                                                        .Build();
            _httpMessageHandler.SetResponse("", HttpStatusCode.InternalServerError);
            Assert.CatchAsync<ApiClientException>(async () => await _target.Create(paymentRequest));
        }

        private async Task AssumeApiResponseInitialized(HttpStatusCode httpStatusCode)
        {
            _apiResponse = Builder<PaymentResponse>.CreateNew()
                                                      .Build();
            var jsonData = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(_apiResponse));
            _httpMessageHandler.SetResponse(jsonData, httpStatusCode);
        }


        private void AssertResult(string url, HttpMethod method, PaymentResponse response)
        {
            _httpMessageHandler.AssertUrl($"{_settings.BaseUrl}/{url}")
                               .Should()
                               .BeTrue();
            _httpMessageHandler.AssertHttpMethod(method)
                               .Should()
                               .BeTrue();
            response.Should()
                    .NotBeNull();

            response.Should().BeEquivalentTo(_apiResponse);
        }



        private IApiClient _target;
        private IApiClientSettings _settings;
        private IHttpClientFactory _httpClientFactory;
        private StubHttpMessageHandler _httpMessageHandler;
        private PaymentResponse _apiResponse;
    }
}
