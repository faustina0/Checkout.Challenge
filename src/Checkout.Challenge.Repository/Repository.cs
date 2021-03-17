using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Challenge.Repository.Entities;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Checkout.Challenge.Repository
{
    public class Repository : IRepository
    {
        private readonly string _dataPath;

        public Repository(string dataPath)
        {
            _dataPath = dataPath;
        }

        public async Task<Guid> InsertPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            var id = Guid.NewGuid();
            paymentTransaction.Id = id;
            var paymentTransactions = await GetData();

            paymentTransactions.Add(paymentTransaction);
            
            var jsonData = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(paymentTransactions));
            
            await File.WriteAllTextAsync(_dataPath, jsonData);

            return id;
        }

        public async Task<PaymentTransaction> GetPaymentTransactionById(string id)
        {
            var data = await GetData();
            return data.FirstOrDefault(x => x.Id.ToString() == id);
        }

        public async Task<IEnumerable<PaymentTransaction>> GetPaymentTransactions()
        {
            return await GetData();
        }

        private async Task<List<PaymentTransaction>> GetData()
        {
            var data =  JsonConvert.DeserializeObject<IEnumerable<PaymentTransaction>>(
                      await File.ReadAllTextAsync(_dataPath));

           return (List<PaymentTransaction>)(data ?? new List<PaymentTransaction>());
        }

    }
}
