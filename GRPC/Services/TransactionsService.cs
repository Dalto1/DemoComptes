using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Data;

namespace GRPC
{
    public class TransactionsService : ProtoTransaction.ProtoTransactionBase
    {
        private readonly DemoComptesContext _context;
        public TransactionsService(DemoComptesContext context)
        {
            _context = context;
        }

        public override async Task<TransactionCreateResponse> TransactionCreate(TransactionCreateParams request, ServerCallContext context)
        {
            TransactionModel transaction = new TransactionModel
            {
                TransactionNumber = request.TransactionNumber,
                TransactionAmount = request.TransactionAmount,
                TransactionDate = request.TransactionDate.ToDateTime(),
                TransactionOrigin = request.TransactionOrigin,
                TransactionDestination = request.TransactionDestination,
                IsValid = request.IsValid
            };
            AccountModel compteOrigine = await _context.Account.FindAsync(transaction.TransactionOrigin);
            AccountModel compteDestination = await _context.Account.FindAsync(transaction.TransactionDestination);
            if (transaction.TransactionAmount < 0)
            {
                transaction.IsValid = false;
            }
            else
            {
                if (compteOrigine != null) compteOrigine.AccountBalance -= transaction.TransactionAmount;
                if (compteDestination != null) compteDestination.AccountBalance += transaction.TransactionAmount;
                _context.Transaction.Add(transaction);
                await _context.SaveChangesAsync();
            }

            return new TransactionCreateResponse
            {
                TransactionNumber = transaction.TransactionNumber,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = Timestamp.FromDateTime(transaction.TransactionDate),
                TransactionOrigin = transaction.TransactionOrigin,
                TransactionDestination = transaction.TransactionDestination,
                IsValid = transaction.IsValid
            };
        }
        public override async Task<TransactionListReponse> TransactionList(Empty request, ServerCallContext context)
        {
            List<TransactionModel> transactions = await _context.Transaction.ToListAsync();
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
        public override async Task<TransactionDeleteAllResponse> TransactionDeleteAll(Empty request, ServerCallContext context)
        {
            List<TransactionModel> transactions = await _context.Transaction.ToListAsync();
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

        public override async Task<TransactionFindResponse> TransactionFind(TransactionFindParams request, ServerCallContext context)
        {
            var transaction = await _context.Transaction.FindAsync(request.TransactionNumber);
            if (transaction == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Transaction introuvable"));
            }
            return new TransactionFindResponse
            {
                TransactionNumber = transaction.TransactionNumber,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = Timestamp.FromDateTime(transaction.TransactionDate),
                TransactionOrigin = transaction.TransactionOrigin,
                TransactionDestination = transaction.TransactionDestination,
                IsValid = transaction.IsValid
            };
        }
        public override async Task<TransactionUpdateResponse> TransactionUpdate(TransactionUpdateParams request, ServerCallContext context)
        {
            TransactionModel transaction = new TransactionModel
            {
                TransactionNumber = request.TransactionNumber,
                TransactionAmount = request.TransactionAmount,
                TransactionDate = request.TransactionDate.ToDateTime(),
                TransactionOrigin = request.TransactionOrigin,
                TransactionDestination = request.TransactionDestination,
                IsValid = request.IsValid
            };
            _context.Entry(transaction).State = EntityState.Modified;

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
            return new TransactionUpdateResponse
            {
                TransactionNumber = transaction.TransactionNumber,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = Timestamp.FromDateTime(transaction.TransactionDate),
                TransactionOrigin = transaction.TransactionOrigin,
                TransactionDestination = transaction.TransactionDestination,
                IsValid = transaction.IsValid
            };
        }
        public override async Task<TransactionDeleteResponse> TransactionDelete(TransactionDeleteParams request, ServerCallContext context)
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
            return new TransactionDeleteResponse
            {
                Success = status
            };
        }
    }
}
