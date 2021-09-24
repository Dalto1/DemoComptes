using Microsoft.Extensions.Logging;

namespace GRPC
{
    public class TransactionsService : Transaction.TransactionBase
    {
        private readonly ILogger<TransactionsService> _logger;
        public TransactionsService(ILogger<TransactionsService> logger)
        {
            _logger = logger;
        }
    }
}
