using System;

namespace Checkout.Challenge.Bank.Api.Dto
{
    public class PaymentResponse
    {
        public int Identifier { get; set; }
        public int AuthorizationCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}