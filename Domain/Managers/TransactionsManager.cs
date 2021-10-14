using Domain.Interfaces;
using Domain.Models;
using System;
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
            /*List<int> ids = new();
            ids.Add(transaction.TransactionOrigin);
            ids.Add(transaction.TransactionDestination);
            AccountModel fromAccount = await _AccountsRepository.FindByAccountId(transaction.TransactionOrigin); 
            AccountModel toAccount = await _AccountsRepository.FindByAccountId(transaction.TransactionDestination);
            if (transaction.TransactionAmount > 0
                && fromAccount != null
                && toAccount != null
                && fromAccount.AccountBalance >= transaction.TransactionAmount)
            {
                fromAccount.AccountBalance -= transaction.TransactionAmount;
                toAccount.AccountBalance += transaction.TransactionAmount;
            }
            else transaction.IsValid = false;
            await _TransactionsRepository.Create(transaction);
            return transaction;*/
            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}
