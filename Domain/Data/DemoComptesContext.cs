using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Domain.Data
{
    public class DemoComptesContext : DbContext
    {
        public DemoComptesContext(DbContextOptions<DemoComptesContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
