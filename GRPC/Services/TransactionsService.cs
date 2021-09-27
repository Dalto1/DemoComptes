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
        public override async Task<ProtoTransactionResponse> TransactionList(Empty request, ServerCallContext context)
        {
            List<Transaction> transactions = await _context.Transaction.ToListAsync();
            ProtoTransactionResponse response = new ProtoTransactionResponse();
            foreach (var trans in transactions)
            {
                response.Transaction.Add(Converter.TransactionToProtoTransactionModel(trans));
            }
            return await Task.FromResult(response);
        }
        public override async Task<Empty> TransactionDeleteAll(Empty request, ServerCallContext context)
        {
            _context.Transaction.RemoveRange(_context.Transaction);
            await _context.SaveChangesAsync();
            return new Empty();
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
        public override async Task<Empty> DeleteTransaction(ProtoTransactionNumber request, ServerCallContext context)
        {
            var transaction = await _context.Transaction.FindAsync(request.TransactionNumber);
            if (transaction == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Transaction introuvable"));
            }
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return new Empty();
        }
    }
}
