using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Challenge.Repository;
using Checkout.Challenge.Services.Model;
using Checkout.Challenge.BankClient;
using Checkout.Challenge.BankClient.Dto;
using Checkout.Challenge.Repository.Entities;

namespace Checkout.Challenge.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankClient _bankClient;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public PaymentService(IBankClient bankClient, IRepository repository, IMapper mapper)
        {
            _bankClient = bankClient;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentResponseModel> ProcessPayment(PaymentRequestModel payment)
        {
            //TODO: have an id provider, also to have an Id if the repository call fails if the repository is responsible for providing the transaction id 
            
            var transactionId = Guid.NewGuid();
            var paymentTransaction = new PaymentTransaction
                                     {
                                         MerchantId = payment.MerchantId,
                                         CardNumber = $"{new string('X', 12)}{payment.Card.Number.Substring(12)}",
                                         CardIssuer = payment.Card.Issuer,
                                         ExpiryDate = payment.Card.ExpiryDate,
                                         NameOnTheCard = payment.Card.NameOnTheCard,
                                         Amount = payment.Amount,
                                         Currency = payment.Currency,
                                         CreateDate = DateTime.Now
                                     };

            try
            {
                var response = await _bankClient.RequestPayment(_mapper.Map<PaymentRequestModel, BankPaymentRequest>(payment));
                paymentTransaction.BankTransactionIdentifier = response.Identifier;
                paymentTransaction.BankAuthorizationCode = response.AuthorizationCode;
                paymentTransaction.StatusCode = response.StatusCode;
                paymentTransaction.StatusMessage = response.StatusMessage;
                paymentTransaction.Approved = response.Approved;
                
                transactionId =  await _repository.InsertPaymentTransaction(paymentTransaction);

            }
            catch(Exception e)
            {
                // TODO: deal with more specific errors.
                // What happens when the call to bank client succeeds but the repository call fails. The payment is authorized so it should return. Maybe a retry mechanism, queues etc
                var error = new Error
                                {
                                    ErrorCode = "500",
                                    ErrorMessage =
                                        "Error while processing the request. Please try again later",
                                    InternalErrorMessage = e.Message,
                                };
                
            }

            var result = _mapper.Map<PaymentTransaction, PaymentResponseModel>(paymentTransaction);
            result.Id = transactionId;
            return result;

        }

        public async Task<PaymentResponseModel> GetPayment(string id)
        {
            var result = await _repository.GetPaymentTransactionById(id);
            return result == null? null: _mapper.Map<PaymentTransaction, PaymentResponseModel>(result);
        }
    }
}