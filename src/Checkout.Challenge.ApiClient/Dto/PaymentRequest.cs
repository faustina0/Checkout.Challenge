using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Challenge.ApiClient.Dto
{
    public class PaymentRequest
    {
        [Required]
        public int MerchantId { get; set; }
        
        [CreditCard(ErrorMessage = "Credit card number is not valid")]
        public string CardNumber { get; set; }
        
        public string CardIssuer { get; set; }
        
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
