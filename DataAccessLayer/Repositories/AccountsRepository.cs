using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using Domain.Models;
using Domain.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly DemoComptesContext _context;
        public AccountsRepository(DemoComptesContext context)
        {
            _context = context;
        }

        public async Task<AccountModel> Create(AccountModel account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }
        public async Task<IEnumerable<AccountModel>> GetAll()
        {
            return await _context.Accounts.ToListAsync();            
        }
        public async Task<bool> DeleteAll()
        {
            DbSet<AccountModel> allAccounts = _context.Accounts;
            if (allAccounts != null)
            {
                _context.Accounts.RemoveRange(allAccounts);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        public async Task<AccountModel> FindByAccountId(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }
        public async Task<AccountModel> Update(int id, AccountModel account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await FindByAccountId(account.AccountId);
            
        }
        public async Task<bool> DeleteByAccountId(int id)
        {
            AccountModel account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactionsByAccountId(int id)
        {
            IEnumerable<TransactionModel> transactions = await _context.Transactions.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id))).ToListAsync();
            return transactions;
        }
        public async Task<bool> DeleteTransactionsByAccountId(int id)
        {
            IQueryable<TransactionModel> accounts = _context.Transactions.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id)));
            if (accounts != null)
            {
                _context.Transactions.RemoveRange(accounts);
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
