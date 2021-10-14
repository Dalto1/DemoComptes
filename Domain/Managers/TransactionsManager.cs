using Domain.Interfaces;
using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Managers
{
    public class TransactionsManager
    {
        public static async Task<TransactionModel> Transfer(IAccountsRepository _AccountsRepository, ITransactionsRepository _TransactionsRepository, TransactionModel transaction)
        {
            AccountModel fromAccount = await _AccountsRepository.FindByAccountId(transaction.TransactionOrigin);
            AccountModel toAccount = await _AccountsRepository.FindByAccountId(transaction.TransactionDestination);
            if (transaction.TransactionAmount > 0 && fromAccount.AccountBalance >= transaction.TransactionAmount)
            {
                if (fromAccount != null) fromAccount.AccountBalance -= transaction.TransactionAmount;
                if (toAccount != null) toAccount.AccountBalance += transaction.TransactionAmount;
            }
            else transaction.IsValid = false;
            await _TransactionsRepository.Create(transaction);
            return transaction;
        }
    }
}
