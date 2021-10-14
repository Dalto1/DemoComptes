using Domain.Interfaces;
using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Managers
{
    public class TransactionsManager
    {
        private readonly IAccountsRepository _AccountsRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        public TransactionsManager(IAccountsRepository accountRepository, ITransactionsRepository transactionsRepository)
        {
            _AccountsRepository = accountRepository;
            _TransactionsRepository = transactionsRepository;
        }
        public async Task<TransactionModel> Transfer(TransactionModel transaction)
        {
            if (transaction.TransactionAmount > 0)
            {
                AccountModel fromAccount = await _AccountsRepository.FindByAccountId(transaction.TransactionOrigin);
                AccountModel toAccount = await _AccountsRepository.FindByAccountId(transaction.TransactionDestination);

                if (fromAccount != null && fromAccount.AccountBalance >= transaction.TransactionAmount)
                {
                    fromAccount.AccountBalance -= transaction.TransactionAmount;
                    if (toAccount != null) toAccount.AccountBalance += transaction.TransactionAmount; //Compte externe
                }
                else transaction.IsValid = false;
            }
            else transaction.IsValid = false;
            await _TransactionsRepository.Create(transaction);
            return transaction;
        }
    }
}
