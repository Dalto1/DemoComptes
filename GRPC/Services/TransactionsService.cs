using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Managers;

namespace GRPC
{
    public class TransactionsService : ProtoTransaction.ProtoTransactionBase
    {
        private readonly IAccountsRepository _AccountsRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        public TransactionsService(IAccountsRepository accountsRepository, ITransactionsRepository transactionsRepository)
        {
            _AccountsRepository = accountsRepository;
            _TransactionsRepository = transactionsRepository;
        }

        public override async Task<TransactionCreateResponse> TransactionCreate(TransactionCreateParams request, ServerCallContext context)
        {
            TransactionModel transaction = new()
            {
                TransactionId = request.TransactionId,
                TransactionAmount = request.TransactionAmount,
                TransactionDate = request.TransactionDate.ToDateTime(),
                TransactionOrigin = request.TransactionOrigin,
                TransactionDestination = request.TransactionDestination,
                IsValid = request.IsValid
            };
            TransactionModel result = await new TransactionsManager(_AccountsRepository, _TransactionsRepository).Transfer(transaction);
            return new TransactionCreateResponse
            {
                TransactionId = result.TransactionId,
                TransactionAmount = result.TransactionAmount,
                TransactionDate = Timestamp.FromDateTime(result.TransactionDate),
                TransactionOrigin = result.TransactionOrigin,
                TransactionDestination = result.TransactionDestination,
                IsValid = result.IsValid
            };
        }
        public override async Task<TransactionGetAllReponse> TransactionGetAll(Empty request, ServerCallContext context)
        {
            IEnumerable<TransactionModel> result = await _TransactionsRepository.GetAll();
            TransactionGetAllReponse response = new();
            foreach (var trans in result)
            {
                TransactionGetAllItem item = new()
                {
                    TransactionId = trans.TransactionId,
                    TransactionAmount = trans.TransactionAmount,
                    TransactionDate = Timestamp.FromDateTime(trans.TransactionDate),
                    TransactionOrigin = trans.TransactionOrigin,
                    TransactionDestination = trans.TransactionDestination,
                    IsValid = trans.IsValid
                };
                response.Transaction.Add(item);
            }
            return await Task.FromResult(response);
        }
        public override async Task<TransactionDeleteAllResponse> TransactionDeleteAll(Empty request, ServerCallContext context)
        {
            bool result = await _TransactionsRepository.DeleteAll();
            return new TransactionDeleteAllResponse { Success = result };
        }

        public override async Task<TransactionFindByTransactionIdResponse> TransactionFindByTransactionId(TransactionFindByTransactionIdParams request, ServerCallContext context)
        {
            TransactionModel result = await _TransactionsRepository.FindByTransactionId(request.TransactionId);
            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Transaction introuvable"));
            }
            return new TransactionFindByTransactionIdResponse
            {
                TransactionId = result.TransactionId,
                TransactionAmount = result.TransactionAmount,
                TransactionDate = Timestamp.FromDateTime(result.TransactionDate),
                TransactionOrigin = result.TransactionOrigin,
                TransactionDestination = result.TransactionDestination,
                IsValid = result.IsValid
            };
        }
        public override async Task<TransactionUpdateResponse> TransactionUpdate(TransactionUpdateParams request, ServerCallContext context)
        {
            TransactionModel transaction = new()
            {
                TransactionId = request.TransactionId,
                TransactionAmount = request.TransactionAmount,
                TransactionDate = request.TransactionDate.ToDateTime(),
                TransactionOrigin = request.TransactionOrigin,
                TransactionDestination = request.TransactionDestination,
                IsValid = request.IsValid
            };
            TransactionModel result = await _TransactionsRepository.Update(request.TransactionId, transaction);
            return new TransactionUpdateResponse
            {
                TransactionId = result.TransactionId,
                TransactionAmount = result.TransactionAmount,
                TransactionDate = Timestamp.FromDateTime(result.TransactionDate),
                TransactionOrigin = result.TransactionOrigin,
                TransactionDestination = result.TransactionDestination,
                IsValid = result.IsValid
            };
        }
        public override async Task<TransactionDeleteByTransactionIdResponse> TransactionDeleteByTransactionId(TransactionDeleteByTransactionIdParams request, ServerCallContext context)
        {
            bool result = await _TransactionsRepository.DeleteByTransactionId(request.TransactionId);
            return new TransactionDeleteByTransactionIdResponse { Success = result };
        }
    }
}
