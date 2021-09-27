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
        public override async Task<ProtoAccountResponse> AccountList(Empty request, ServerCallContext context)
        {
            List<Account> accounts = await _context.Account.ToListAsync();
            ProtoAccountResponse response = new ProtoAccountResponse();
            foreach (var acc in accounts)
            {
                response.Account.Add(Converter.AccountToProtoCompteModel(acc));
            }
            return await Task.FromResult(response);
        }
        public override async Task<Empty> AccountDeleteAll(Empty request, ServerCallContext context)
        {
            _context.Account.RemoveRange(_context.Account);
            await _context.SaveChangesAsync();
            return new Empty();
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
        public override async Task<Empty> AccountDelete(ProtoAccountNumber request, ServerCallContext context)
        {
            var account = await _context.Account.FindAsync(request.AccountNumber);
            if (account == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Compte introuvable"));
            }
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return new Empty();
        }
        public override async Task<ProtoTransactionResponse> GetTransactionsByAccount(ProtoAccountNumber request, ServerCallContext context)
        {
            List<Transaction> transactions = await _context.Transaction.Where(s => (s.TransactionOrigin.Equals(request.AccountNumber) || s.TransactionDestination.Equals(request.AccountNumber))).ToListAsync();

            ProtoTransactionResponse response = new ProtoTransactionResponse();
            foreach (var trans in transactions)
            {
                response.Transaction.Add(Converter.TransactionToProtoTransactionModel(trans));
            }
            return await Task.FromResult(response);
        }
        public override async Task<Empty> DeleteTransactionsByAccount(ProtoAccountNumber request, ServerCallContext context)
        {
            _context.Transaction.RemoveRange(_context.Transaction.Where(s => (s.TransactionOrigin.Equals(request.AccountNumber) || s.TransactionDestination.Equals(request.AccountNumber))));
            await _context.SaveChangesAsync();
            return new Empty();
        }
    }
}
