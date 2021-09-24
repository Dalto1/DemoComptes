using Microsoft.EntityFrameworkCore;
using REST.Models;

namespace REST.Data
{
    public class RESTContext : DbContext
    {
        public RESTContext(DbContextOptions<RESTContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
