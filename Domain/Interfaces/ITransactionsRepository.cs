using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<TransactionModel> TransactionCreate(TransactionModel transaction);
        Task<IEnumerable<TransactionModel>> TransactionList();
        Task<bool> TransactionDeleteAll();

        Task<TransactionModel> TransactionFind(int id);
        Task<TransactionModel> TransactionUpdate(int id, TransactionModel transaction);
        Task<bool> TransactionDelete(int id);
    }
}
