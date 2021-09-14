using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetC.Data;
using ProjetC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetC.Controllers
{
    [Route("api/comptes")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ProjetCContext _context;
        public AccountController(ProjetCContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> AccountCreate(Account account)
        {
            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.AccountNumber }, account);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> AccountList()
        {
            return await _context.Account.ToListAsync();
        }
        [HttpPut]
        public string AccountMultiUpdate()
        {
            return "Update Multiple Accounts";
        } //TODO Méthode pour updater plusieurs accounts
        [HttpDelete]
        public string AccountDeleteAll()
        {
            return "Update Multiple Accounts";
        } //TODO Méthode pour effacer tout les comptes

        [HttpPost("{id}")]
        public string Error()
        {
            return "Commande inexistante";
        } //TODO Transformer cette commande en erreur standard
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

        [HttpGet("{account1}/transfert/{account2}/{amount}")]
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

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.AccountNumber == id);
        }
    }
}
