using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain.Data
{
    public class DemoComptesContext : DbContext
    {
        public DemoComptesContext(DbContextOptions<DemoComptesContext> options) : base(options) { }

        public DbSet<AccountModel> Account { get; set; }
        public DbSet<TransactionModel> Transaction { get; set; }
    }
}
