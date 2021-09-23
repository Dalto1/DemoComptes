using Microsoft.EntityFrameworkCore;
using ProjetC.Models;

namespace ProjetC.Data
{
    public class ProjetCContext : DbContext
    {
        public ProjetCContext(DbContextOptions<ProjetCContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
