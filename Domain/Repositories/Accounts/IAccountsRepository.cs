using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories.Accounts
{
    public interface IAccountsRepository
    {
        Task<AccountModel> AccountCreate(AccountModel account);
        Task<IEnumerable<AccountModel>> AccountList();
        Task<bool> AccountDeleteAll();

        Task<AccountModel> AccountFind(int id);
        Task<AccountModel> AccountUpdate(int id, AccountModel account);
        Task<bool> AccountDelete(int id);

        Task<IEnumerable<TransactionModel>> GetTransactionsByAccount(int id);
        Task<bool> DeleteTransactionsByAccount(int id);
    }
}
