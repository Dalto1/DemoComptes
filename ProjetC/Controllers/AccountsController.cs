﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetC.Data;
using ProjetC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetC.Controllers
{
    [Route("api/comptes")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ProjetCContext _context;
        public AccountsController(ProjetCContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> AccountCreate(Account account)
        {
            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("AccountFind", new { id = account.AccountNumber }, account);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> AccountList()
        {
            return await _context.Account.ToListAsync();
        }
        [HttpDelete]
        public async Task<IActionResult> AccountDeleteAll()
        {
            _context.Account.RemoveRange(_context.Account);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> AccountFind(int id)
        {
            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> AccountUpdate(int id, Account account)
        {
            if (id != account.AccountNumber)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> AccountDelete(int id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByAccount(int id)
        {
            return await _context.Transaction.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id))).ToListAsync();
        }
        [HttpDelete("{id}/transactions")]
        public async Task<IActionResult> DeleteTransactionsByAccount(int id)
        {
            _context.Transaction.RemoveRange(_context.Transaction.Where(s => (s.TransactionOrigin.Equals(id) || s.TransactionDestination.Equals(id))));
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}")]
        [HttpPost("{id}/transactions")]
        [HttpPost("transactions/{id}")]
        [HttpPut]
        [HttpPut("{id}/transactions")]
        [HttpPut("transactions")]
        public BadRequestResult Error()
        {
            return BadRequest();
        }
        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.AccountNumber == id);
        }
    }
}
