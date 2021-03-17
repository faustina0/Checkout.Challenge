
using System.Threading.Tasks;
using Checkout.Challenge.ApiClient.Dto;

namespace Checkout.Challenge.ApiClient
{
    public interface IApiClient
    {
        Task<PaymentResponse> Create(PaymentRequest payment);
        Task<PaymentResponse> Get(string id);
    }
}