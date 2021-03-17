using System;

namespace Checkout.Challenge.BankClient.Config
{
    public interface IBankClientSettings
    {
        string BaseUrl { get; set; }
    }
}