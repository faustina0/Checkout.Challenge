using System;
using System.Threading.Tasks;
using Checkout.Challenge.BankClient.Dto;

namespace Checkout.Challenge.BankClient
{
    public interface IBankClient
    {
        Task<BankPaymentResponse> RequestPayment(BankPaymentRequest payment);
    }
}