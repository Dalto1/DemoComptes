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
        public async Task<List<AccountModel>> FindByAccountIds(List<int> ids)
        {
            List<AccountModel> result = new();
            result.Insert(0, await _context.Accounts.FindAsync(ids[0]));
            result.Insert(1, await _context.Accounts.FindAsync(ids[1]));
            return result;
        }
        public async Task<AccountModel> Update(AccountModel account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return account;
            
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

        public async Task<List<TransactionModel>> GetTransactionsByAccountId(int id)
        {
            return await _context.Transactions.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id))).ToListAsync();
        }
        public async Task<bool> DeleteTransactionsByAccountId(int id)
        {
            List<TransactionModel> accounts = await _context.Transactions.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id))).ToListAsync();
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
