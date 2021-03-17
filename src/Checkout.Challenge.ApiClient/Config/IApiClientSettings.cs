using System;

namespace Checkout.Challenge.ApiClient.Config
{
    public interface IApiClientSettings
    {
        string BaseUrl { get; set; }
    }
}