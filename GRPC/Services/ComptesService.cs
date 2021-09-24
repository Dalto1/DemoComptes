using Microsoft.Extensions.Logging;

namespace GRPC
{
    public class ComptesService : Compte.CompteBase
    {
        private readonly ILogger<ComptesService> _logger;
        public ComptesService(ILogger<ComptesService> logger)
        {
            _logger = logger;
        }
    }
}
