using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Repositories.Accounts;
using Domain.Data;

namespace REST.Controllers
{
    [Route("api/comptes")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsRepository _AccountsRepository;
        
        public AccountsController(IAccountsRepository accountRepository)
        {
            _AccountsRepository = accountRepository;
        }
        /*private readonly DemoComptesContext _context;
        IAccountsRepository _AccountsRepository = new AccountsRepository(_context);*/

        [HttpPost]
        public async Task<ActionResult<AccountModel>> AccountCreate(AccountModel account)
        {
            AccountModel result = await _AccountsRepository.AccountCreate(account);
            if (result!=null) return CreatedAtAction("Account Created", new { id = result.AccountNumber }, result);
            else return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountModel>>> AccountList()
        {
            IEnumerable<AccountModel> result = await _AccountsRepository.AccountList();
            if (result != null) return Ok(result);
            else return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> AccountDeleteAll()
        {
            bool result = await _AccountsRepository.AccountDeleteAll();
            if (result) return NoContent();
            else return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountModel>> AccountFind(int id)
        {
            AccountModel result = await _AccountsRepository.AccountFind(id);
            if (result != null) return Ok(result);
            else return NotFound();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> AccountUpdate(int id, AccountModel account)
        {
            AccountModel result = await _AccountsRepository.AccountUpdate(id, account);
            if (result != null) return Ok(result);
            else return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> AccountDelete(int id) 
        {
            bool result = await _AccountsRepository.AccountDelete(id);
            if (result) return NoContent();
            else return NotFound();
        }

        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactionsByAccount(int id)
        {
            IEnumerable<TransactionModel> result = await _AccountsRepository.GetTransactionsByAccount(id);
            if (result != null) return Ok(result);
            else return NotFound();
        }
        [HttpDelete("{id}/transactions")]
        public async Task<IActionResult> DeleteTransactionsByAccount(int id)
        {
            bool result = await _AccountsRepository.DeleteTransactionsByAccount(id);
            if (result) return NoContent();
            else return NotFound();
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
    }
}
