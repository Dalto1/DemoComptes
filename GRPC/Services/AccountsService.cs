using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace GRPC
{
    public class AccountsService : ProtoAccount.ProtoAccountBase
    {
        private readonly IAccountsRepository _AccountsRepository;
        public AccountsService(IAccountsRepository accountsRepository)
        {
            _AccountsRepository = accountsRepository;
        }
        public override async Task<AccountCreateResponse> AccountCreate(AccountCreateParams request, ServerCallContext context)
        {
            AccountModel account = new()
            {
                AccountId = request.AccountId,
                AccountNumber = request.AccountNumber,
                AccountBalance = request.AccountBalance,
                AccountCreationDate = request.AccountCreationDate.ToDateTime(),
                AccountHolderFirstName = request.AccountHolderFirstName,
                AccountHolderLastName = request.AccountHolderLastName,
                IsActive = request.IsActive
            };
            await _AccountsRepository.Create(account);
            return new AccountCreateResponse
            {
                AccountId = request.AccountId,
                AccountNumber = request.AccountNumber,
                AccountBalance = request.AccountBalance,
                AccountCreationDate = request.AccountCreationDate,
                AccountHolderFirstName = request.AccountHolderFirstName,
                AccountHolderLastName = request.AccountHolderLastName,
                IsActive = request.IsActive
            };
        }
        public override async Task<AccountGetAllResponse> AccountGetAll(Empty request, ServerCallContext context)
        {
            IEnumerable<AccountModel> result = await _AccountsRepository.GetAll();
            AccountGetAllResponse response = new();
            foreach (var account in result)
            {
                AccountGetAllItem item = new()
                {
                    AccountId = account.AccountId,
                    AccountNumber = account.AccountNumber,
                    AccountBalance = account.AccountBalance,
                    AccountCreationDate = Timestamp.FromDateTime(account.AccountCreationDate),
                    AccountHolderFirstName = account.AccountHolderFirstName,
                    AccountHolderLastName = account.AccountHolderLastName,
                    IsActive = account.IsActive
                };
                response.Account.Add(item);
            }
            return await Task.FromResult(response);
        }
        public override async Task<AccountDeleteAllResponse> AccountDeleteAll(Empty request, ServerCallContext context)
        {
            bool result = await _AccountsRepository.DeleteAll();
            return new AccountDeleteAllResponse { Success = result };
        }

        public override async Task<AccountFindByAccountIdResponse> AccountFindByAccountId(AccountFindByAccountIdParams request, ServerCallContext context)
        {
            AccountModel result = await _AccountsRepository.FindByAccountId(request.AccountId);

            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Compte introuvable"));
            }
            return new AccountFindByAccountIdResponse
            {
                AccountId = result.AccountId,
                AccountNumber = result.AccountNumber,
                AccountBalance = result.AccountBalance,
                AccountCreationDate = Timestamp.FromDateTime(result.AccountCreationDate),
                AccountHolderFirstName = result.AccountHolderFirstName,
                AccountHolderLastName = result.AccountHolderLastName,
                IsActive = result.IsActive
            };
        }
        public override async Task<AccountUpdateResponse> AccountUpdate(AccountUpdateParams request, ServerCallContext context)
        {
            AccountModel account = new()
            {
                AccountId = request.AccountId,
                AccountNumber = request.AccountNumber,
                AccountBalance = request.AccountBalance,
                AccountCreationDate = request.AccountCreationDate.ToDateTime(),
                AccountHolderFirstName = request.AccountHolderFirstName,
                AccountHolderLastName = request.AccountHolderLastName,
                IsActive = request.IsActive
            };
            AccountModel result = await _AccountsRepository.Update(account);
            return new AccountUpdateResponse
            {
                AccountId = result.AccountId,
                AccountNumber = result.AccountNumber,
                AccountBalance = result.AccountBalance,
                AccountCreationDate = Timestamp.FromDateTime(result.AccountCreationDate),
                AccountHolderFirstName = result.AccountHolderFirstName,
                AccountHolderLastName = result.AccountHolderLastName,
                IsActive = result.IsActive
            };
        }
        public override async Task<AccountDeleteByAccountIdResponse> AccountDeleteByAccountId(AccountDeleteByAccountIdParams request, ServerCallContext context)
        {
            bool result = await _AccountsRepository.DeleteByAccountId(request.AccountId);
            return new AccountDeleteByAccountIdResponse { Success = result };
        }

        public override async Task<TransactionGetAllReponse> GetAllTransactionsByAccountId(GetTransactionsByAccountIdParams request, ServerCallContext context)
        {
            IEnumerable<TransactionModel> result = await _AccountsRepository.GetTransactionsByAccountId(request.AccountId);
            TransactionGetAllReponse response = new();
            foreach (var transaction in result)
            {
                TransactionGetAllItem item = new()
                {
                    TransactionId = transaction.TransactionId,
                    TransactionAmount = transaction.TransactionAmount,
                    TransactionDate = Timestamp.FromDateTime(transaction.TransactionDate),
                    TransactionOrigin = transaction.TransactionOrigin,
                    TransactionDestination = transaction.TransactionDestination
                };
                response.Transaction.Add(item);
            }
            return await Task.FromResult(response);
        }
        public override async Task<TransactionDeleteAllResponse> DeleteAllTransactionsByAccountId(DeleteTransactionsByAccountIdParams request, ServerCallContext context)
        {
            bool result = await _AccountsRepository.DeleteTransactionsByAccountId(request.AccountId);
            return new TransactionDeleteAllResponse { Success = result };
        }
    }
}
