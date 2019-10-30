using Microsoft.EntityFrameworkCore;
using DBRepository.Interfaces;

namespace DBRepository.Factories
{
    public class RepositoryContextFactory : IRepositoryContextFactory
    {
        public RepositoryContext CreateDBContext(string connectionString)
        {
            var dbContextOptions = SetupDbContextOptions(connectionString);
            return new RepositoryContext(dbContextOptions);
        }

        private DbContextOptions<RepositoryContext> SetupDbContextOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(RepositoryContextFactory).Assembly.GetName().Name);
            });
            return optionsBuilder.Options;
        }
    }
}
