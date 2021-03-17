using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Challenge.BankClient.Dto
{
    public class BankPaymentRequest
    {
        public int MerchantId { get; set; }
        public string CardNumber { get; set; }
        public string CardIssuer { get; set; }
        public string CardExpiryDate { get; set; }
        public string NameOnTheCard { get; set; }
        public int CardCvv { get; set; }
        public string CardPostCode { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreateDate { get; set; }
        

    }
}
