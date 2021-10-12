using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAccountsRepository
    {
        Task<AccountModel> Create(AccountModel account);
        Task<IEnumerable<AccountModel>> GetAll();
        Task<bool> DeleteAll();

        Task<AccountModel> FindByAccountId(int id);
        Task<AccountModel> Update(int id, AccountModel account);
        Task<bool> DeleteByAccountId(int id);

        Task<IEnumerable<TransactionModel>> GetTransactionsByAccountId(int id);
        Task<bool> DeleteTransactionsByAccountId(int id);
    }
}
