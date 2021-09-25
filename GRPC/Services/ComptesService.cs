using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Librairies.Data;
using Librairies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRPC
{
    public class ComptesService : Compte.CompteBase
    {
        private readonly ProjectCContext _context;
        public ComptesService(ProjectCContext context)
        {
            _context = context;
        }

        public override async Task AccountList(Empty request, IServerStreamWriter<CompteModel> responseStream, ServerCallContext context)
        {
            List<Account> comptes = await _context.Account.ToListAsync();

            foreach (var comp in comptes)
            {
                await responseStream.WriteAsync(AccountToCompteModel(comp));
            }
        }
        public override async Task<CompteModel> AccountCreate(CompteModel request, ServerCallContext context)
        {
            _context.Account.Add(CompteModelToAccount(request));
            await _context.SaveChangesAsync();

            return await Task.FromResult(request);
        }
        public CompteModel AccountToCompteModel(Account account)
        {
            return new CompteModel
            {
                AccountNumber = account.AccountNumber,
                AccountBalance = account.AccountBalance,
                AccountCreationDate = Timestamp.FromDateTime(DateTime.UtcNow),
                AccountHolderFirstName = account.AccountHolderFirstName,
                AccountHolderLastName = account.AccountHolderLastName,
                IsActive = account.isActive
            };
        }
        public Account CompteModelToAccount(CompteModel model)
        {
            return new Account
            {
                AccountNumber = model.AccountNumber,
                AccountBalance = model.AccountBalance,
                AccountCreationDate = DateTime.UtcNow,
                AccountHolderFirstName = model.AccountHolderFirstName,
                AccountHolderLastName = model.AccountHolderLastName,
                isActive = model.IsActive
            };
        }
    }
}
