using System;

namespace Checkout.Challenge.BankClient.Config
{
    public class BankClientSettings : IBankClientSettings
    {
        public string BaseUrl { get; set; }
    }
}
