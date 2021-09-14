using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetC.Data;
using ProjetC.Models;

namespace ProjetC.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsContext _context;
        public TransactionsController(TransactionsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> TransactionCreate(Transaction transaction)
        {
            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("TransactionFind", new { id = transaction.TransactionNumber }, transaction);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> TransactionList()
        {
            return await _context.Transaction.ToListAsync();
        }
        [HttpDelete]
        public async Task<IActionResult> TransactionDeleteAll()
        {
            _context.Transaction.RemoveRange(_context.Transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}")]
        public BadRequestResult Error()
        {
            return BadRequest();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> TransactionFind(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> TransactionUpdate(int id, Transaction transaction)
        {
            if (id != transaction.TransactionNumber)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
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

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*[HttpGet("{account1}/transfert/{account2}/{amount}")]
        public async Task<string> Transfer(int account1, int account2, int amount)
        {
            var accountOriginal = await _context.Account.FindAsync(account1);
            var accountDestination = await _context.Account.FindAsync(account2);

            if (accountOriginal == null || accountDestination == null) return "Un ou plusieurs compte(s) est(sont) introuvable(s).";
            if (!accountOriginal.isActive || !accountDestination.isActive) return "Un ou plusieurs compte(s) est(sont) désactivé(s).";
            if (accountOriginal.AccountBalance < amount) return "Solde insufisant";
            if (amount < 0) return "Le solde du transfert doit être positif";

            accountDestination.AccountBalance += amount;
            accountOriginal.AccountBalance -= amount;

            await _context.SaveChangesAsync();

            return "Le transfert du compte #" + accountOriginal.AccountNumber + " au compte #" + accountDestination.AccountNumber + " d'un montant de " + amount + "$ est completé";
        } // TODO Utiliser un HTTPPost pour fabriquer une transaction
        */

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionNumber == id);
        }
    }
}
