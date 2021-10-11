using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;

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

        [HttpPost]
        public async Task<ActionResult<AccountModel>> AccountCreate(AccountModel account)
        {
            AccountModel result = await _AccountsRepository.AccountCreate(account);
            if (result == null) return NoContent();
            return CreatedAtAction("AccountFind", new { id = result.AccountNumber }, result);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountModel>>> AccountList()
        {
            IEnumerable<AccountModel> result = await _AccountsRepository.AccountList();
            if (result == null) return NoContent();
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> AccountDeleteAll()
        {
            bool result = await _AccountsRepository.AccountDeleteAll();
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountModel>> AccountFind(int id)
        {
            AccountModel result = await _AccountsRepository.AccountFind(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> AccountUpdate(int id, AccountModel account)
        {
            AccountModel result = await _AccountsRepository.AccountUpdate(id, account);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> AccountDelete(int id) 
        {
            bool result = await _AccountsRepository.AccountDelete(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactionsByAccount(int id)
        {
            IEnumerable<TransactionModel> result = await _AccountsRepository.GetTransactionsByAccount(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}/transactions")]
        public async Task<IActionResult> DeleteTransactionsByAccount(int id)
        {
            bool result = await _AccountsRepository.DeleteTransactionsByAccount(id);
            if (!result) return NotFound();
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
    }
}
