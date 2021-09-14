using Microsoft.EntityFrameworkCore;
using ProjetC.Models;

namespace ProjetC.Data
{
    public class TransactionsContext : DbContext
    {
        public TransactionsContext (DbContextOptions<TransactionsContext> options) : base(options) { }

        public DbSet<Transaction> Transaction { get; set; }
    }
}
