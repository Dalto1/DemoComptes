using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<TransactionModel> Create(TransactionModel transaction);
        Task<IEnumerable<TransactionModel>> GetAll();
        Task<bool> DeleteAll();

        Task<TransactionModel> FindByTransactionId(int id);
        Task<TransactionModel> Update(int id, TransactionModel transaction);
        Task<bool> DeleteByTransactionId(int id);
    }
}
