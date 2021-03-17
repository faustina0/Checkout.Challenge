using System;

namespace Checkout.Challenge.Repository.Entities
{
    public class PaymentTransaction
    {   
        public Guid Id { get; set; }
        public int MerchantId { get; set; }
        public string CardNumber { get; set; }
        public string CardIssuer { get; set; }
        public string ExpiryDate { get; set; }
        public string NameOnTheCard { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int BankTransactionIdentifier { get; set; }
        public int BankAuthorizationCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool Approved { get; set; }
        public DateTime CreateDate { get; set; }
        
    }

    
}
