using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetC.Data;
using ProjetC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetC.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ProjetCContext _context;
        public AccountController(ProjetCContext context)
        {
            _context = context;
        }
        [HttpGet("api/health")]
        public string Health()
        {
            return "System is up";
        }
        [HttpGet("api/initialize")]
        public string InitializeDB()
        {
            var compte1 = new Account { AccountNumber = 11235, AccountBalance = 99, isActive = true };
            var compte2 = new Account { AccountNumber = 12441, AccountBalance = 27, isActive = true };
            var compte3 = new Account { AccountNumber = 99123, AccountBalance = 10, isActive = false, AccountCreationDate = new DateTime(2015, 5, 17) };
            _context.Comptes.Add(compte1);
            _context.Comptes.Add(compte2);
            _context.Comptes.Add(compte3);
            _context.SaveChanges();
            return "Les données ont été initialisées.";
        }
        [HttpGet("api/comptes/list")]
        public async Task<ActionResult<IEnumerable<Account>>> ListeComptes()
        {
            return await _context.Comptes.ToListAsync();
        }
        [HttpGet("api/comptes/info/{CompteNum}")]
        public async Task<ActionResult<Account>> InfoCompte(int CompteNum)
        {
            var compte = await _context.Comptes.FindAsync(CompteNum);

            if (compte == null)
            {
                return NotFound();
            }

            return compte;
        }
        [HttpGet("api/comptes/transfert/{account1}/{account2}/{amount}")]
        public string Transfer(int account1, int account2, int amount)
        {
            var accountOriginal = _context.Comptes.Find(account1);
            var accountDestination = _context.Comptes.Find(account2);

            if (accountOriginal == null || accountDestination == null) return "Un ou plusieurs compte(s) est(sont) introuvable(s).";
            if (!accountOriginal.isActive || !accountDestination.isActive) return "Un ou plusieurs compte(s) est(sont) désactivé(s).";
            if (accountOriginal.AccountBalance < amount) return "Solde insufisant";
            if (amount < 0) return "Le solde du transfert doit être positif";

            accountDestination.AccountBalance += amount;
            accountOriginal.AccountBalance -= amount;

            _context.SaveChanges();

            return "Le transfert du compte #" + accountOriginal.AccountNumber + " au compte #" + accountDestination.AccountNumber + " d'un montant de " + amount + "$ est completé";
        }
    }
}
