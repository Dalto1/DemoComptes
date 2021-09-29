using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Domain.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRPC.Tools;

namespace GRPC
{
    public class AccountsService : ProtoAccount.ProtoAccountBase
    {
        private readonly ProjectCContext _context;
        public AccountsService(ProjectCContext context)
        {
            _context = context;
        }
        public override async Task<ProtoAccountModel> AccountCreate(ProtoAccountModel request, ServerCallContext context)
        {
            _context.Account.Add(Converter.ProtoAccountModelToAccount(request));
            await _context.SaveChangesAsync();

            return await Task.FromResult(request);
        }
        public override async Task<ProtoAccountModelList> AccountList(Empty request, ServerCallContext context)
        {
            List<Account> accounts = await _context.Account.ToListAsync();
            ProtoAccountModelList response = new ProtoAccountModelList();
            foreach (var acc in accounts)
            {
                response.Account.Add(Converter.AccountToProtoCompteModel(acc));
            }
            return await Task.FromResult(response);
        }
        public override async Task<ProtoAccountDeleteAllStatus> AccountDeleteAll(Empty request, ServerCallContext context)
        {
            List<Account> accounts = await _context.Account.ToListAsync();
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
            return new ProtoAccountDeleteAllStatus
            {
                Success = status,
                Deleted = accountsCount
            };
        }

        public override async Task<ProtoAccountModel> AccountFind(ProtoAccountNumber request, ServerCallContext context)
        {
            var account = await _context.Account.FindAsync(request.AccountNumber);

            if (account == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Compte introuvable"));
            }
            return await Task.FromResult(Converter.AccountToProtoCompteModel(account));
        }
        public override async Task<ProtoAccountModel> AccountUpdate(ProtoAccountModel request, ServerCallContext context)
        {
            _context.Entry(Converter.ProtoAccountModelToAccount(request)).State = EntityState.Modified;

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
            return await Task.FromResult(request);
        }
        public override async Task<ProtoAccountDeleteStatus> AccountDelete(ProtoAccountNumber request, ServerCallContext context)
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
            return new ProtoAccountDeleteStatus
            {
                Success = status
            };
        }

        public override async Task<ProtoTransactionModelList> GetTransactionsByAccount(ProtoAccountNumber request, ServerCallContext context)
        {
            List<Transaction> transactions = await _context.Transaction.Where(s => (s.TransactionOrigin.Equals(request.AccountNumber) || s.TransactionDestination.Equals(request.AccountNumber))).ToListAsync();

            ProtoTransactionModelList response = new ProtoTransactionModelList();
            foreach (var trans in transactions)
            {
                response.Transaction.Add(Converter.TransactionToProtoTransactionModel(trans));
            }
            return await Task.FromResult(response);
        }
        public override async Task<ProtoTransactionDeleteAllStatus> DeleteTransactionsByAccount(ProtoAccountNumber request, ServerCallContext context)
        {
            List<Transaction> transactions = await _context.Transaction.Where(s => (s.TransactionOrigin.Equals(request.AccountNumber) || s.TransactionDestination.Equals(request.AccountNumber))).ToListAsync();
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
            return new ProtoTransactionDeleteAllStatus
            {
                Success = status,
                Deleted = transactionsCount
            };
        }
    }
}
