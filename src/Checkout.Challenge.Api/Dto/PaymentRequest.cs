using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using Checkout.Challenge.Api.Attributes;

namespace Checkout.Challenge.Api.Dto
{
    public class PaymentRequest
    {
        [Required]
        public int MerchantId { get; set; }
        [Required]
        [CreditCard(ErrorMessage = "Credit card number is not valid")]
        public string CardNumber { get; set; }
        [Required]
        public string CardIssuer { get; set; }
        [CardExpiryDate(ErrorMessage = "Card expiry date not valid")]
        public string ExpiryDate { get; set; }
        [Required]
        [Range(100, 999, ErrorMessage = "CVV must be a 3 digit number")]
        public int Cvv { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string NameOnTheCard { get; set; }
        [Required]
        public string PostCode { get; set; }
    }
}
