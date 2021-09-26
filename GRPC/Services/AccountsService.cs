using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Librairies.Data;
using Librairies.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRPC
{
    public class AccountsService : ProtoAccount.ProtoAccountBase
    {
        private readonly ProjectCContext _context;
        public AccountsService(ProjectCContext context)
        {
            _context = context;
        }
        public override async Task<ProtoAccountModel> AccountCreate(ProtoAccountModel request, ServerCallContext context)
        {
            _context.Account.Add(ProtoAccountModelToAccount(request));
            await _context.SaveChangesAsync();

            return await Task.FromResult(request);
        }
        public override async Task AccountList(Empty request, IServerStreamWriter<ProtoAccountModel> responseStream, ServerCallContext context)
        {
            //TODO Retourner un tableau
            List<Account> comptes = await _context.Account.ToListAsync();

            foreach (var comp in comptes)
            {
                await responseStream.WriteAsync(AccountToProtoCompteModel(comp));
            }
        }
        public override async Task<Empty> AccountDeleteAll(Empty request, ServerCallContext context)
        {
            _context.Account.RemoveRange(_context.Account);
            await _context.SaveChangesAsync();
            return new Empty();
        }

        public override async Task<ProtoAccountModel> AccountFind(ProtoAccountNumber request, ServerCallContext context)
        {
            var account = await _context.Account.FindAsync(request.AccountNumber);

            //TODO CHECK IF NULL
            /*if (account == null)
            {
                return null;
            }*/

            return await Task.FromResult(AccountToProtoCompteModel(account));
        }
        public override async Task<ProtoAccountModel> AccountUpdate(ProtoAccountModel request, ServerCallContext context)
        {
            //TODO CHECK IF NULL or NOT FOUND
            /*if (id != account.AccountNumber)
            {
                return BadRequest();
            }*/

            _context.Entry(ProtoAccountModelToAccount(request)).State = EntityState.Modified;

            //try
            //{
                await _context.SaveChangesAsync();
            /*}
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();*/
            return await Task.FromResult(request);
        }
        public override async Task<Empty> AccountDelete(ProtoAccountNumber request, ServerCallContext context)
        {
            var account = await _context.Account.FindAsync(request.AccountNumber);
            //TODO CHECK IF NULL
            /*if (account == null)
            {
                return NotFound();
            }*/

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return new Empty();
        }

        public override  Task GetTransactionsByAccount(ProtoAccountNumber request, IServerStreamWriter<ProtoAccountModel> responseStream, ServerCallContext context)
        {
            //TODO
            return null;
        }
        public override async Task<Empty> DeleteTransactionsByAccount(ProtoAccountNumber request, ServerCallContext context)
        {
            _context.Transaction.RemoveRange(_context.Transaction.Where(s => (s.TransactionOrigin.Equals(request.AccountNumber) || s.TransactionDestination.Equals(request.AccountNumber))));
            await _context.SaveChangesAsync();
            return new Empty();
        }

        public ProtoAccountModel AccountToProtoCompteModel(Account account)
        {
            return new ProtoAccountModel
            {
                AccountNumber = account.AccountNumber,
                AccountBalance = account.AccountBalance,
                //TODO CHECK UTC
                //AccountCreationDate = Timestamp.FromDateTime(DateTime.SpecifyKind(account.AccountCreationDate, DateTimeKind.Utc)),
                AccountCreationDate = Timestamp.FromDateTime(account.AccountCreationDate),
                AccountHolderFirstName = account.AccountHolderFirstName,
                AccountHolderLastName = account.AccountHolderLastName,
                IsActive = account.IsActive
            };
        }
        public Account ProtoAccountModelToAccount(ProtoAccountModel model)
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
    }
}
