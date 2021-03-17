using AutoMapper;
using Checkout.Challenge.BankClient.Dto;

namespace Checkout.Challenge.BankClient
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentResponse, BankPaymentResponse>()
                .ForMember(d => d.Approved, opt => opt.Ignore());
        }
    }
}
