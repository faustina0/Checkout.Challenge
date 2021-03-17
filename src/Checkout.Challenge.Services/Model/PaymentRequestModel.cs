using System;

namespace Checkout.Challenge.Services.Model
{
    public class PaymentRequestModel
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public Card Card { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreateDate { get; set; }
    }
}