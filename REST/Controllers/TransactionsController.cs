using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Repositories.Transactions;
using Domain.Data;

namespace REST.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsRepository _TransactionsRepository;
        public TransactionsController(ITransactionsRepository transactionRepository)
        {
            _TransactionsRepository = transactionRepository;
        }
        /*private readonly DemoComptesContext _context;
        ITransactionsRepository _TransactionsRepository = new TransactionsRepository(_context);*/

        [HttpPost]
        public async Task<ActionResult<TransactionModel>> TransactionCreate(TransactionModel transaction)
        {
            TransactionModel result = await _TransactionsRepository.TransactionCreate(transaction);
            if (result != null) return CreatedAtAction("Transaction Created", new { id = result.TransactionNumber }, result);
            else return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> TransactionList()
        {
            IEnumerable<TransactionModel> result = await _TransactionsRepository.TransactionList();
            if (result != null) return Ok(result);
            else return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> TransactionDeleteAll()
        {
            bool result = await _TransactionsRepository.TransactionDeleteAll();
            if (result) return NoContent();
            else return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionModel>> TransactionFind(int id)
        {
            TransactionModel result = await _TransactionsRepository.TransactionFind(id);
            if (result != null) return Ok(result);
            else return NotFound();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> TransactionUpdate(int id, TransactionModel transaction)
        {
            TransactionModel result = await _TransactionsRepository.TransactionUpdate(id, transaction);
            if (result != null) return Ok(result);
            else return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> TransactionDelete(int id)
        {
            bool result = await _TransactionsRepository.TransactionDelete(id);
            if (result) return NoContent();
            else return NotFound();
        }

        [HttpPut]
        [HttpPost("{id}")]
        public BadRequestResult Error()
        {
            return BadRequest();
        }
    }
}
