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
        Task<List<AccountModel>> FindByAccountIds(List<int> ids);
        Task<AccountModel> Update(AccountModel account);
        Task<bool> DeleteByAccountId(int id);

        Task<List<TransactionModel>> GetTransactionsByAccountId(int id);
        Task<bool> DeleteTransactionsByAccountId(int id);
    }
}
