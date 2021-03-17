using System;

namespace Checkout.Challenge.Services.Model
{
    public class Card
    {
        public string Number { get; set; }
        public string Issuer { get; set; }
        public string ExpiryDate { get; set; }
        public int Cvv { get; set; }
        public string NameOnTheCard { get; set; }
        public string PostCode { get; set; }
    }
}
