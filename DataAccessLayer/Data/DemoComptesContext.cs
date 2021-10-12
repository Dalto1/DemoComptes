using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace DataAccessLayer.Data
{
    public class DemoComptesContext : DbContext
    {
        public DemoComptesContext(DbContextOptions<DemoComptesContext> options) : base(options) { }

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
    }
}
