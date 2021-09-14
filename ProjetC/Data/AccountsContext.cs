using Microsoft.EntityFrameworkCore;
using ProjetC.Models;

namespace ProjetC.Data
{
    public class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
    }
}
