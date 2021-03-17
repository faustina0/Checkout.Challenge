using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Challenge.ApiClient.Config
{
    public class ApiClientSettings: IApiClientSettings
    {
        public string BaseUrl { get; set; }
    }
}
