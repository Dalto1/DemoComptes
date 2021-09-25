using Microsoft.EntityFrameworkCore;
using REST.Models;

namespace REST.Data
{
    public class ProjectCContext : DbContext
    {
        public ProjectCContext(DbContextOptions<ProjectCContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
