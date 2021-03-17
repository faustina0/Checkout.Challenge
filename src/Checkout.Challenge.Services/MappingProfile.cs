using AutoMapper;
using Checkout.Challenge.BankClient.Dto;
using Checkout.Challenge.Repository.Entities;
using Checkout.Challenge.Services.Model;

namespace Checkout.Challenge.Services
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequestModel, BankPaymentRequest>()
                .ForMember(d => d.CreateDate, opt => opt.Ignore())
                .ForMember(d => d.CardNumber, s =>s.MapFrom(m => m.Card.Number))
                .ForMember(d => d.CardIssuer, s => s.MapFrom(m => m.Card.Issuer))
                .ForMember(d => d.CardExpiryDate, s => s.MapFrom(m => m.Card.ExpiryDate))
                .ForMember(d => d.CardCvv, s => s.MapFrom(m => m.Card.Cvv))
                .ForMember(d => d.CardPostCode, s => s.MapFrom(m => m.Card.PostCode))
                .ForMember(d => d.NameOnTheCard, s => s.MapFrom(m => m.Card.NameOnTheCard));

            CreateMap<PaymentTransaction, PaymentResponseModel>();
        }
    }
}
