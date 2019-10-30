using Models;
using Microsoft.EntityFrameworkCore;

namespace DBRepository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<LastUpdate> LastUpdates { get; set; }
        public DbSet<Rate> Rates { get; set; }        
        public DbSet<User> Users { get; set; }
    }
}
