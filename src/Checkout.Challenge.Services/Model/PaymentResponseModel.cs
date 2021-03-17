using System;

namespace Checkout.Challenge.Services.Model
{
    public class PaymentResponseModel
    {
        public Guid Id { get; set; }
        public int MerchantId { get; set; }
        public string CardNumber { get; set; }
        public string NameOnTheCard { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool Approved { get; set; }
        public DateTime CreateDate { get; set; }
    }
}