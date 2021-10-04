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
        public async Task<TransactionModel> TransactionCreate(TransactionModel transaction)
        {
            try
            {
                _context.Transaction.Add(transaction);
                await _context.SaveChangesAsync();
                return await TransactionFind(transaction.TransactionNumber);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<IEnumerable<TransactionModel>> TransactionList()
        {
            try
            {
                return await _context.Transaction.ToListAsync();
            }
            catch (Exception)
            {

                return null;
            }

        }
        public async Task<bool> TransactionDeleteAll()
        {
            try
            {
                _context.Transaction.RemoveRange(_context.Transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TransactionModel> TransactionFind(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null) return null;
            else return transaction;

        }
        public async Task<TransactionModel> TransactionUpdate(int id, TransactionModel transaction)
        {
            try
            {
                _context.Entry(transaction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return await TransactionFind(transaction.TransactionNumber);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<bool> TransactionDelete(int id)
        {
            try
            {
                TransactionModel transaction = await _context.Transaction.FindAsync(id);
                _context.Transaction.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
