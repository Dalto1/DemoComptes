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

        public async Task<AccountModel> AccountCreate(AccountModel account)
        {
            try
            {
                _context.Account.Add(account);
                await _context.SaveChangesAsync();
                return await AccountFind(account.AccountNumber);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<IEnumerable<AccountModel>> AccountList()
        {
            try
            {
                return await _context.Account.ToListAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public async Task<bool> AccountDeleteAll()
        {
            try
            {
                _context.Account.RemoveRange(_context.Account);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AccountModel> AccountFind(int id)
        {
            var account = await _context.Account.FindAsync(id);

            if (account == null) return null;
            else return account;
        }
        public async Task<AccountModel> AccountUpdate(int id, AccountModel account)
        {
            try
            {
                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return await AccountFind(account.AccountNumber);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> AccountDelete(int id)
        {
            try
            {
                AccountModel account = await _context.Account.FindAsync(id);
                _context.Account.Remove(account);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactionsByAccount(int id)
        {
            try
            {
                IEnumerable<TransactionModel> transactions = await _context.Transaction.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id))).ToListAsync();
                return transactions;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> DeleteTransactionsByAccount(int id)
        {
            try
            {
                _context.Transaction.RemoveRange(_context.Transaction.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id))));
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
