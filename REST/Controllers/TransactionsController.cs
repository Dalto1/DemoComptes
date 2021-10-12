using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace REST.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IAccountsRepository _AccountsRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        public TransactionsController(IAccountsRepository accountsRepository, ITransactionsRepository transactionRepository)
        {
            _AccountsRepository = accountsRepository;
            _TransactionsRepository = transactionRepository;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionModel>> Create(TransactionModel transaction)
        {
            TransactionModel result = null;
            AccountModel compteOrigine = await _AccountsRepository.FindByAccountId(transaction.TransactionOrigin);
            AccountModel compteDestination = await _AccountsRepository.FindByAccountId(transaction.TransactionDestination);
            if (transaction.TransactionAmount < 0)
            {
                transaction.IsValid = false;
            }
            else
            {
                if (compteOrigine != null) compteOrigine.AccountBalance -= transaction.TransactionAmount;
                if (compteDestination != null) compteDestination.AccountBalance += transaction.TransactionAmount;
                result = await _TransactionsRepository.Create(transaction);
            }

            if (result == null) return NoContent();
            return CreatedAtAction("TransactionFind", new { id = result.TransactionId }, result);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetAll()
        {
            IEnumerable<TransactionModel> result = await _TransactionsRepository.GetAll();
            if (result == null) return NoContent();
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            bool result = await _TransactionsRepository.DeleteAll();
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionModel>> FindByTransactionId(int id)
        {
            TransactionModel result = await _TransactionsRepository.FindByTransactionId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TransactionModel transaction)
        {
            TransactionModel result = await _TransactionsRepository.Update(id, transaction);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByTransactionId(int id)
        {
            bool result = await _TransactionsRepository.DeleteByTransactionId(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut]
        [HttpPost("{id}")]
        public BadRequestResult Error()
        {
            return BadRequest();
        }
    }
}
