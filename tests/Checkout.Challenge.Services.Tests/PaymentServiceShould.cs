using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Challenge.BankClient;
using Checkout.Challenge.BankClient.Dto;
using Checkout.Challenge.Repository;
using Checkout.Challenge.Repository.Entities;
using Checkout.Challenge.Services.Model;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Checkout.Challenge.Services.Tests
{
    [TestFixture]
    internal class PaymentServiceShould
    {
        [SetUp]
        public void SetUp()
        {
            _bankClient = Substitute.For<IBankClient>();
            _repository = Substitute.For<IRepository>();
            _mapper = Substitute.For<IMapper>();

            _cardNumber = "1234567891234567";
            _paymentRequest = Builder<PaymentRequestModel>.CreateNew()
                                                          .With(x => x.Card = Builder<Card>.CreateNew()
                                                                         .Build())
                                                          .With(x => x.Card.Number = _cardNumber)
                                                          .Build();
            _bankPaymentRequest = Builder<BankPaymentRequest>.CreateNew()
                                                             .Build();

            _paymentResponseModel = Builder<PaymentResponseModel>.CreateNew()
                                                                 .Build();

            _bankPaymentResponse = Builder<BankPaymentResponse>.CreateNew()
                                                               .Build();

            _paymentTransaction = Builder<PaymentTransaction>.CreateNew()
                                                             .Build();
            _mapper.Map<PaymentRequestModel, BankPaymentRequest>(_paymentRequest)
                   .Returns(_bankPaymentRequest);
            _mapper.Map<PaymentTransaction, PaymentResponseModel>(Arg.Any<PaymentTransaction>())
                   .Returns(_paymentResponseModel);
            

            _target = new PaymentService(_bankClient, _repository, _mapper);
        }

        [Test]
        public void UseTheBankClientToRequestPayment()
        {
            _target.ProcessPayment(_paymentRequest);
            _bankClient.Received(1)
                       .RequestPayment(_bankPaymentRequest);
        }

        [Test]
        public async Task ReturnEvenWhenErrorOnPaymentRequest()
        {
            _bankClient.RequestPayment(_bankPaymentRequest)
                       .Returns(Task.FromException<BankPaymentResponse>(new Exception("bad request")));
            var result = await _target.ProcessPayment(_paymentRequest);
            result.Should()
                  .NotBeNull();
        }

        [Test]
        public async Task SaveThePaymentTransactionOnPaymentRequest()
        {
            _bankClient.RequestPayment(_bankPaymentRequest)
                       .Returns(_bankPaymentResponse);
            await _target.ProcessPayment(_paymentRequest);
            await _repository.Received(1)
                             .InsertPaymentTransaction(Arg.Is<PaymentTransaction>(
                                 x => x.CardNumber == "XXXXXXXXXXXX4567" && x.Id != null
                                      && x.StatusCode == _bankPaymentResponse.StatusCode));
        }

        [Test]
        public async Task ReturnsTheCorrectDataOnPaymentRequest()
        {
            var id = Guid.NewGuid();
            _bankClient.RequestPayment(_bankPaymentRequest)
                       .Returns(_bankPaymentResponse);
            _repository.InsertPaymentTransaction(Arg.Any<PaymentTransaction>())
                       .Returns(id);
            var result = await _target.ProcessPayment(_paymentRequest);

            result.Should()
                  .BeAssignableTo<PaymentResponseModel>();
            result.Id.Should()
                  .Be(id);
        }


        [Test]
        public async Task UseTheRepositoryToGetPayment()
        {
            var id = Guid.NewGuid().ToString();
            await _target.GetPayment(id);
            await _repository.Received(1)
                       .GetPaymentTransactionById(id);
        }

        [Test]
        public async Task ReturnTheCorrectDataOnGetPayment()
        {
            var id = Guid.NewGuid().ToString();
            _repository.GetPaymentTransactionById(id)
                       .Returns(_paymentTransaction);
            var result = await _target.GetPayment(id);
            result.Should()
                  .Be(_paymentResponseModel);

        }

        


        private IPaymentService _target;
        private IBankClient _bankClient;
        private IRepository _repository;
        private PaymentRequestModel _paymentRequest;
        private IMapper _mapper;
        private BankPaymentRequest _bankPaymentRequest;
        private PaymentResponseModel _paymentResponseModel;
        private string _cardNumber;
        private BankPaymentResponse _bankPaymentResponse;
        private PaymentTransaction _paymentTransaction;
    }
}