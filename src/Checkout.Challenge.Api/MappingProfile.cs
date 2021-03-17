using System;
using AutoMapper;
using Checkout.Challenge.Api.Dto;
using Checkout.Challenge.Services.Model;

namespace Checkout.Challenge.Api
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequest, PaymentRequestModel>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreateDate, opt => opt.Ignore())
                .ForMember(d => d.Card,
                    opt => opt.MapFrom(s => new Card
                                            {
                                                Number = s.CardNumber,
                                                Issuer = s.CardIssuer,
                                                ExpiryDate = s.ExpiryDate,
                                                Cvv = s.Cvv,
                                                NameOnTheCard = s.NameOnTheCard,
                                                PostCode = s.PostCode
                                            }));





            CreateMap<PaymentResponseModel, PaymentResponse>();


        }
    }
}
