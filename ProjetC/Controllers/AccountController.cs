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
            var compte3 = new Account { AccountNumber = 99123, AccountBalance = 10, isActive = false };
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
        [HttpGet("api/comptes/transfert/{num1}/{num2}/{montant}")]
        public string Transfer(int account1, int account2, int amount)
        {
            var accountOriginal = _context.Comptes.Find(account1);
            var accountDestination = _context.Comptes.Find(account2);
            if (accountOriginal == null || accountDestination == null) return "Un ou plusieurs compte(s) est(sont) introuvable(s).";
            if (!accountOriginal.isActive || !accountDestination.isActive) return "Un ou plusieurs compte(s) est(sont) désactivé(s).";
            if (accountOriginal.AccountBalance < amount) return "Solde insufisant";

            accountDestination.AccountBalance += amount;
            accountOriginal.AccountBalance -= amount;

            _context.SaveChanges();

            return "Transfert complété";
        }
        [HttpGet("api/comptes/transfertv2/{num1}/{num2}/{montant}")]
        public async ValueTask<string> Transfer2(int fromAccount, int toAccount, int amount)
        {
            var originalAccount = await _context.Comptes.FindAsync(fromAccount);
            var destinationAccount = await _context.Comptes.FindAsync(toAccount);

            string result;
            if (originalAccount == null || destinationAccount == null) result = "Un ou plusieurs compte(s) est(sont) introuvable(s).";
            if (!originalAccount.isActive || !destinationAccount.isActive) result = "Un ou plusieurs compte(s) est(sont) désactivé(s).";
            if (originalAccount.AccountBalance < amount) result = "Solde insufisant";

            destinationAccount.AccountBalance += amount;
            originalAccount.AccountBalance -= amount;

            _context.SaveChanges();

            return null;
        }
    }
}
