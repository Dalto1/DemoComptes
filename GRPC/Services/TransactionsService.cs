using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Librairies.Data;
using Librairies.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            _context.Transaction.Add(ProtoTransactionModelToTransaction(request));
            await _context.SaveChangesAsync();

            return await Task.FromResult(request);
        }
        public override async Task TransactionList(Empty request, IServerStreamWriter<ProtoTransactionModel> responseStream, ServerCallContext context)
        {
            //TODO Retourner un tableau
            List<Transaction> transactions = await _context.Transaction.ToListAsync();

            foreach (var trans in transactions)
            {
                await responseStream.WriteAsync(TransactionToProtoTransactionModel(trans));
            }
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

            //TODO CHECK IF NULL
            /*if (transaction == null)
            {
                return null;
            }*/

            return await Task.FromResult(TransactionToProtoTransactionModel(transaction));
        }
        public override async Task<ProtoTransactionModel> TransactionUpdate(ProtoTransactionModel request, ServerCallContext context)
        {
            //TODO CHECK IF NULL or NOT FOUND
            /*if (id != transaction.TransactionNumber)
            {
                return BadRequest();
            }*/

            _context.Entry(ProtoTransactionModelToTransaction(request)).State = EntityState.Modified;

            //try
            //{
            await _context.SaveChangesAsync();
            /*}
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();*/
            return await Task.FromResult(request);
        }
        public override async Task<Empty> DeleteTransaction(ProtoTransactionNumber request, ServerCallContext context)
        {
            var transaction = await _context.Transaction.FindAsync(request.TransactionNumber);
            //TODO CHECK IF NULL
            /*if (account == null)
            {
                return NotFound();
            }*/

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return new Empty();
        }

        public ProtoTransactionModel TransactionToProtoTransactionModel(Transaction transaction)
        {
            return new ProtoTransactionModel
            {
                TransactionNumber = transaction.TransactionNumber,
                TransactionAmount = transaction.TransactionAmount,
                //TODO CHECK UTC
                //AccountCreationDate = Timestamp.FromDateTime(DateTime.SpecifyKind(account.AccountCreationDate, DateTimeKind.Utc)),
                TransactionDate = Timestamp.FromDateTime(transaction.TransactionDate),
                TransactionOrigin = transaction.TransactionOrigin,
                TransactionDestination = transaction.TransactionDestination,
                IsValid = transaction.IsValid
            };
        }
        public Transaction ProtoTransactionModelToTransaction(ProtoTransactionModel model)
        {
            return new Transaction
            {
                TransactionNumber = model.TransactionNumber,
                TransactionAmount = model.TransactionAmount,
                TransactionDate = model.TransactionDate.ToDateTime(),
                TransactionOrigin = model.TransactionOrigin,
                TransactionDestination = model.TransactionDestination,
                IsValid = model.IsValid
            };
        }
    }
}
