using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using Domain.Models;
using Domain.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly DemoComptesContext _context;
        public TransactionsRepository(DemoComptesContext context)
        {
            _context = context;
        }
        public async Task<TransactionModel> Create(TransactionModel transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        public async Task<IEnumerable<TransactionModel>> GetAll()
        {
            return await _context.Transactions.ToListAsync();
        }
        public async Task<bool> DeleteAll()
        {
            DbSet<TransactionModel> allTransactions = _context.Transactions;
            if (allTransactions != null)
            {
                _context.Transactions.RemoveRange(allTransactions);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<TransactionModel> FindByTransactionId(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }
        public async Task<TransactionModel> Update(int id, TransactionModel transaction)
        {
            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await FindByTransactionId(transaction.TransactionId);
        }
        public async Task<bool> DeleteByTransactionId(int id)
        {
            TransactionModel transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
