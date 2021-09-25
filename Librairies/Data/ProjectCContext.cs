using Microsoft.EntityFrameworkCore;
using Librairies.Models;

namespace Librairies.Data
{
    public class ProjectCContext : DbContext
    {
        public ProjectCContext(DbContextOptions<ProjectCContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
