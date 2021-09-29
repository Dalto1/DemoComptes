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
    public class TransactionsService : ProtoTransaction.ProtoTransactionBase
    {
        private readonly ProjectCContext _context;
        public TransactionsService(ProjectCContext context)
        {
            _context = context;
        }

        public override async Task<ProtoTransactionModel> TransactionCreate(ProtoTransactionModel request, ServerCallContext context)
        {
            _context.Transaction.Add(Converter.ProtoTransactionModelToTransaction(request));
            await _context.SaveChangesAsync();
            return await Task.FromResult(request);
        }
        public override async Task<ProtoTransactionModelList> TransactionList(Empty request, ServerCallContext context)
        {
            List<Transaction> transactions = await _context.Transaction.ToListAsync();
            ProtoTransactionModelList response = new ProtoTransactionModelList();
            foreach (var trans in transactions)
            {
                response.Transaction.Add(Converter.TransactionToProtoTransactionModel(trans));
            }
            return await Task.FromResult(response);
        }
        public override async Task<ProtoTransactionDeleteAllStatus> TransactionDeleteAll(Empty request, ServerCallContext context)
        {
            List<Transaction> transactions = await _context.Transaction.ToListAsync();
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

        public override async Task<ProtoTransactionModel> TransactionFind(ProtoTransactionNumber request, ServerCallContext context)
        {
            var transaction = await _context.Transaction.FindAsync(request.TransactionNumber);
            if (transaction == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Transaction introuvable"));
            }
            return await Task.FromResult(Converter.TransactionToProtoTransactionModel(transaction));
        }
        public override async Task<ProtoTransactionModel> TransactionUpdate(ProtoTransactionModel request, ServerCallContext context)
        {
            _context.Entry(Converter.ProtoTransactionModelToTransaction(request)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transaction.Any(e => e.TransactionNumber == request.TransactionNumber))
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Transaction introuvable"));
                }
                else
                {
                    throw;
                }
            }
            return await Task.FromResult(request);
        }
        public override async Task<ProtoTransactionDeleteStatus> DeleteTransaction(ProtoTransactionNumber request, ServerCallContext context)
        {
            bool status = false;
            var transaction = await _context.Transaction.FindAsync(request.TransactionNumber);
            if (transaction != null)
            {
                try
                {
                    _context.Transaction.Remove(transaction);
                    await _context.SaveChangesAsync();
                    status = true;
                }
                catch { }
            }
            return new ProtoTransactionDeleteStatus
            {
                Success = status
            };
        }
    }
}
