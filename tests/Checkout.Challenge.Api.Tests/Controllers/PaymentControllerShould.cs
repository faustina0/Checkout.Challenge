using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Challenge.Api.Controllers;
using Checkout.Challenge.Api.Dto;
using Checkout.Challenge.BankClient.Dto;
using Checkout.Challenge.Services;
using Checkout.Challenge.Services.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using PaymentRequest = Checkout.Challenge.Api.Dto.PaymentRequest;
using PaymentResponse = Checkout.Challenge.Api.Dto.PaymentResponse;

namespace Checkout.Challenge.Api.Tests.Controllers
{
    [TestFixture]
    public class PaymentControllerShould
    {
        [SetUp]
        public void SetUp()
        {
            _service = Substitute.For<IPaymentService>();
            _mapper = Substitute.For<IMapper>();

            _target = new PaymentController(_service, _mapper)
                      {
                          ControllerContext = new ControllerContext
                                              {
                                                  HttpContext = new DefaultHttpContext()
                                              }
                      };

            AssumeModelIsInitialized();
        }

        [Test]
        public async Task FailIfModelStateIsInvalid()
        {
            _target.ModelState.AddModelError("Name", "Name is required.");

            var result = await _target.Create(new PaymentRequest());

            result.Result.Should()
                  .BeOfType<BadRequestObjectResult>();

            _mapper.Received(0)
                   .Map<PaymentRequest, PaymentRequestModel>(Arg.Any<PaymentRequest>());
            await _service.Received(0)
                          .ProcessPayment(Arg.Any<PaymentRequestModel>());
        }

        [Test]
        public async Task UseMapperToProcessCreateRequest()
        {
            await _target.Create(_paymentRequest);
            _mapper.Received(1)
                   .Map<PaymentRequest, PaymentRequestModel>(_paymentRequest);
        }

        [Test]
        public async Task UseServiceToProcessTheCreateRequest()
        {
            _mapper.Map<PaymentRequest, PaymentRequestModel>(_paymentRequest)
                   .Returns(_paymentRequestModel);
            await _target.Create(_paymentRequest);
            await _service.Received(1)
                          .ProcessPayment(_paymentRequestModel);
        }

        [Test]
        public async Task ReturnTheCorrectResponseOnCreate()
        {
            _mapper.Map<PaymentRequest, PaymentRequestModel>(_paymentRequest)
                   .Returns(_paymentRequestModel);
            _service.ProcessPayment(_paymentRequestModel)
                    .Returns(_paymentResponseModel);
            _mapper.Map<PaymentResponseModel, PaymentResponse>(_paymentResponseModel)
                   .Returns(_paymentResponse);

            var response = await _target.Create(_paymentRequest);

            response.Result.Should()
                    .BeOfType<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Should()
                  .NotBeNull();
            result.Value.Should()
                  .BeEquivalentTo(_paymentResponse);
        }


        [Test]
        public async Task UseServiceToProcessTheGetPayment()
        {
            var id = Guid.NewGuid().ToString();
            await _target.Get(id);
            await _service.Received(1)
                          .GetPayment(id);
        }


        [Test]
        public async Task ReturnTheCorrectResponseOnGet()
        {
            var id = Guid.NewGuid().ToString();
            _service.GetPayment(id).Returns(_paymentResponseModel);
            _mapper.Map<PaymentResponseModel, PaymentResponse>(_paymentResponseModel)
                   .Returns(_paymentResponse);
            var response = await _target.Get(id);

            response.Result.Should()
                    .BeOfType<OkObjectResult>();
            var result = (OkObjectResult)response.Result;

            result.Should()
                  .NotBeNull();
            result.Value.Should()
                  .BeEquivalentTo(_paymentResponse);
        }

        [Test]
        public async Task ReturnNotFoundWhenTheIdCannotBeFoundOnGet()
        {

            var id = Guid.NewGuid().ToString();
            _service.GetPayment(id).Returns((PaymentResponseModel)null);
            var response = await _target.Get(id);

            response.Result.Should()
                    .BeOfType<NotFoundObjectResult>();
          
        }


        private void AssumeModelIsInitialized()
        {
            _paymentRequest = new PaymentRequest
                              {
                                  MerchantId = 12020,
                                  CardNumber = "1234567891234567",
                                  ExpiryDate = "10/21",
                                  CardIssuer = "VISA",
                                  Cvv = 111,
                                  Amount = 12.10M,
                                  NameOnTheCard = "Mr Test",
                                  PostCode = "SS1 9SS"
                              };

            _paymentRequestModel = new PaymentRequestModel
                                   {
                                       MerchantId = 12020,
                                       Card = new Card
                                              {
                                                  Number = "1234567891234567",
                                                  Issuer = "VISA",
                                                  Cvv = 111,
                                                  NameOnTheCard = "Mr Test",
                                                  PostCode = "SS1 9SS"
                                              },
                                       Amount = 12.10M,
                                   };

            _paymentResponseModel = new PaymentResponseModel
                                    {
                                        MerchantId = 12020,
                                        CardNumber = "XXXXXXXXXXXX4567",
                                        Approved = true,
                                        StatusCode = "authorized"
                                    };

            _paymentResponse = new PaymentResponse
                               {
                                   MerchantId = 12020,
                                   CardNumber = "XXXXXXXXXXXX4567",
                                   Approved = true,
                                   StatusCode = "authorized"
                               };
        }

        private PaymentController _target;
        private IPaymentService _service;
        private IMapper _mapper;
        private PaymentRequest _paymentRequest;
        private PaymentRequestModel _paymentRequestModel;
        private PaymentResponseModel _paymentResponseModel;
        private PaymentResponse _paymentResponse;
    }
}