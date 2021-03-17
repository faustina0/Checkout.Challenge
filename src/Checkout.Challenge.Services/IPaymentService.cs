using System;
using System.Threading.Tasks;
using Checkout.Challenge.Services.Model;

namespace Checkout.Challenge.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseModel> ProcessPayment(PaymentRequestModel payment);
        Task<PaymentResponseModel> GetPayment(string id);
    }
}