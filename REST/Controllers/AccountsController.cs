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
        public async Task<ActionResult<AccountModel>> Create(AccountModel account)
        {
            AccountModel result = await _AccountsRepository.Create(account);
            if (result == null) return NoContent();
            return CreatedAtAction("FindByAccountId", new { id = result.AccountId }, result);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountModel>>> GetAll()
        {
            IEnumerable<AccountModel> result = await _AccountsRepository.GetAll();
            if (result == null) return NoContent();
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            bool result = await _AccountsRepository.DeleteAll();
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountModel>> FindByAccountId(int id)
        {
            AccountModel result = await _AccountsRepository.FindByAccountId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AccountModel account)
        {
            AccountModel result = await _AccountsRepository.Update(id, account);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByAccountId(int id) 
        {
            bool result = await _AccountsRepository.DeleteByAccountId(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactionsByAccountId(int id)
        {
            IEnumerable<TransactionModel> result = await _AccountsRepository.GetTransactionsByAccountId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}/transactions")]
        public async Task<IActionResult> DeleteTransactionsByAccountId(int id)
        {
            bool result = await _AccountsRepository.DeleteTransactionsByAccountId(id);
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
