using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
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
            transaction.IsValid = false;
            if (transaction.TransactionAmount > 0)
            {
                List<int> ids = new();
                ids.Add(transaction.TransactionOrigin);
                ids.Add(transaction.TransactionDestination);
                List<AccountModel> accounts = await _AccountsRepository.FindByAccountsIds(ids);
                AccountModel accountOrigin = accounts.Find(x => x.AccountId == transaction.TransactionOrigin);
                AccountModel accountDestination = accounts.Find(x => x.AccountId == transaction.TransactionDestination);
                if (accountOrigin != null &&
                    accountDestination != null &&
                    accountOrigin.AccountBalance >= transaction.TransactionAmount)
                {
                    accountOrigin.AccountBalance -= transaction.TransactionAmount;
                    accountDestination.AccountBalance += transaction.TransactionAmount;
                    transaction.IsValid = true;
                }
            }
            await _TransactionsRepository.Create(transaction);
            return transaction;
        }
    }
}
