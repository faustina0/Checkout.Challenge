using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.Challenge.Repository.Entities;

namespace Checkout.Challenge.Repository
{
    public interface IRepository
    {
        Task<Guid> InsertPaymentTransaction(PaymentTransaction paymentTransaction);
        Task<PaymentTransaction> GetPaymentTransactionById(string id);
        Task<IEnumerable<PaymentTransaction>> GetPaymentTransactions();

    }
}