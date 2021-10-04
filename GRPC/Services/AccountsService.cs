using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using DataAccessLayer.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRPC
{
    public class AccountsService : ProtoAccount.ProtoAccountBase
    {
        private readonly DemoComptesContext _context;
        public AccountsService(DemoComptesContext context)
        {
            _context = context;
        }
        public override async Task<AccountCreateResponse> AccountCreate(AccountCreateParams request, ServerCallContext context)
        {
            AccountModel account = new AccountModel
            {
                AccountNumber = request.AccountNumber,
                AccountBalance = request.AccountBalance,
                AccountCreationDate = request.AccountCreationDate.ToDateTime(),
                AccountHolderFirstName = request.AccountHolderFirstName,
                AccountHolderLastName = request.AccountHolderLastName,
                IsActive = request.IsActive
            };
            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return new AccountCreateResponse
            {
                AccountNumber = request.AccountNumber,
                AccountBalance = request.AccountBalance,
                AccountCreationDate = request.AccountCreationDate,
                AccountHolderFirstName = request.AccountHolderFirstName,
                AccountHolderLastName = request.AccountHolderLastName,
                IsActive = request.IsActive
            };
        }
        public override async Task<AccountListResponse> AccountList(Empty request, ServerCallContext context)
        {
            List<AccountModel> accounts = await _context.Account.ToListAsync();
            AccountListResponse response = new AccountListResponse();
            foreach (var acc in accounts)
            {
                AccountListItem item = new AccountListItem
                {
                    AccountNumber = acc.AccountNumber,
                    AccountBalance = acc.AccountBalance,
                    AccountCreationDate = Timestamp.FromDateTime(acc.AccountCreationDate),
                    AccountHolderFirstName = acc.AccountHolderFirstName,
                    AccountHolderLastName = acc.AccountHolderLastName,
                    IsActive = acc.IsActive
                };
                response.Account.Add(item);
            }
            return await Task.FromResult(response);
        }
        public override async Task<AccountDeleteAllResponse> AccountDeleteAll(Empty request, ServerCallContext context)
        {
            List<AccountModel> accounts = await _context.Account.ToListAsync();
            int accountsCount = accounts.Count;
            bool status = false;
            if (accountsCount > 0)
            {
                try
                {
                    _context.Account.RemoveRange(accounts);
                    await _context.SaveChangesAsync();
                    status = true;
                }
                catch { }
            }
            return new AccountDeleteAllResponse
            {
                Success = status,
                Deleted = accountsCount
            };
        }

        public override async Task<AccountFindResponse> AccountFind(AccountFindParams request, ServerCallContext context)
        {
            var account = await _context.Account.FindAsync(request.AccountNumber);

            if (account == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Compte introuvable"));
            }
            return new AccountFindResponse
            {
                AccountNumber = account.AccountNumber,
                AccountBalance = account.AccountBalance,
                AccountCreationDate = Timestamp.FromDateTime(account.AccountCreationDate),
                AccountHolderFirstName = account.AccountHolderFirstName,
                AccountHolderLastName = account.AccountHolderLastName,
                IsActive = account.IsActive
            };
        }
        public override async Task<AccountUpdateResponse> AccountUpdate(AccountUpdateParams request, ServerCallContext context)
        {
            AccountModel account = new AccountModel
            {
                AccountNumber = request.AccountNumber,
                AccountBalance = request.AccountBalance,
                AccountCreationDate = request.AccountCreationDate.ToDateTime(),
                AccountHolderFirstName = request.AccountHolderFirstName,
                AccountHolderLastName = request.AccountHolderLastName,
                IsActive = request.IsActive
            };
            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Account.Any(e => e.AccountNumber == request.AccountNumber))
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Compte introuvable"));
                }
                else
                {
                    throw;
                }
            }
            return new AccountUpdateResponse
            {
                AccountNumber = account.AccountNumber,
                AccountBalance = account.AccountBalance,
                AccountCreationDate = Timestamp.FromDateTime(account.AccountCreationDate),
                AccountHolderFirstName = account.AccountHolderFirstName,
                AccountHolderLastName = account.AccountHolderLastName,
                IsActive = account.IsActive
            };
        }
        public override async Task<AccountDeleteResponse> AccountDelete(AccountDeleteParams request, ServerCallContext context)
        {
            bool status = false;
            var account = await _context.Account.FindAsync(request.AccountNumber);
            if (account != null)
            {
                try
                {
                    _context.Account.Remove(account);
                    await _context.SaveChangesAsync();
                    status = true;
                }
                catch { }
            }
            return new AccountDeleteResponse
            {
                Success = status
            };
        }

        public override async Task<TransactionListReponse> GetTransactionsByAccount(GetTransactionsByAccountParams request, ServerCallContext context)
        {
            List<TransactionModel> transactions = await _context.Transaction.Where(s => (s.TransactionOrigin.Equals(request.AccountNumber) || s.TransactionDestination.Equals(request.AccountNumber))).ToListAsync();

            TransactionListReponse response = new TransactionListReponse();
            foreach (var trans in transactions)
            {
                TransactionListItem item = new TransactionListItem
                {
                    TransactionNumber = trans.TransactionNumber,
                    TransactionAmount = trans.TransactionAmount,
                    TransactionDate = Timestamp.FromDateTime(trans.TransactionDate),
                    TransactionOrigin = trans.TransactionOrigin,
                    TransactionDestination = trans.TransactionDestination,
                    IsValid = trans.IsValid
                };
                response.Transaction.Add(item);
            }
            return await Task.FromResult(response);
        }
        public override async Task<TransactionDeleteAllResponse> DeleteTransactionsByAccount(DeleteTransactionsByAccountParams request, ServerCallContext context)
        {
            List<TransactionModel> transactions = await _context.Transaction.Where(s => (s.TransactionOrigin.Equals(request.AccountNumber) || s.TransactionDestination.Equals(request.AccountNumber))).ToListAsync();
            int transactionsCount = transactions.Count;
            bool status = false;
            if (transactionsCount > 0)
            {
                try
                {
                    _context.Transaction.RemoveRange(transactions);
                    await _context.SaveChangesAsync();
                    status = true;
                }
                catch { }
            }
            return new TransactionDeleteAllResponse
            {
                Success = status,
                Deleted = transactionsCount
            };
        }
    }
}
