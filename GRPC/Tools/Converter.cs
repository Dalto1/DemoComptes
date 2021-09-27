using Domain.Models;
using Google.Protobuf.WellKnownTypes;

namespace GRPC.Tools
{
    public class Converter
    {
        public static ProtoAccountModel AccountToProtoCompteModel(Account account)
        {
            return new ProtoAccountModel
            {
                AccountNumber = account.AccountNumber,
                AccountBalance = account.AccountBalance,
                AccountCreationDate = Timestamp.FromDateTime(account.AccountCreationDate),
                AccountHolderFirstName = account.AccountHolderFirstName,
                AccountHolderLastName = account.AccountHolderLastName,
                IsActive = account.IsActive
            };
        }
        public static Account ProtoAccountModelToAccount(ProtoAccountModel model)
        {
            return new Account
            {
                AccountNumber = model.AccountNumber,
                AccountBalance = model.AccountBalance,
                AccountCreationDate = model.AccountCreationDate.ToDateTime(),
                AccountHolderFirstName = model.AccountHolderFirstName,
                AccountHolderLastName = model.AccountHolderLastName,
                IsActive = model.IsActive
            };
        }
        public static ProtoTransactionModel TransactionToProtoTransactionModel(Transaction transaction)
        {
            return new ProtoTransactionModel
            {
                TransactionNumber = transaction.TransactionNumber,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = Timestamp.FromDateTime(transaction.TransactionDate),
                TransactionOrigin = transaction.TransactionOrigin,
                TransactionDestination = transaction.TransactionDestination,
                IsValid = transaction.IsValid
            };
        }
        public static Transaction ProtoTransactionModelToTransaction(ProtoTransactionModel model)
        {
            return new Transaction
            {
                TransactionNumber = model.TransactionNumber,
                TransactionAmount = model.TransactionAmount,
                TransactionDate = model.TransactionDate.ToDateTime(),
                TransactionOrigin = model.TransactionOrigin,
                TransactionDestination = model.TransactionDestination,
                IsValid = model.IsValid
            };
        }
    }
}
