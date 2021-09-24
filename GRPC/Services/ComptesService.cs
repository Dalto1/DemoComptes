using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
