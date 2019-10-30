using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using DBRepository.Interfaces;
using DBRepository.Factories;
using DBRepository.Repositories;
using Options;
using Microsoft.Extensions.Options;

namespace DBRepository.Initialize
{
    public static class Initializer
    {
        private static IServiceCollection _services;
        private static IConfiguration _configuration;
        private static ILogger _logger;

        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _services = services;
            _configuration = configuration;
            MigrateDatabase();
            RegisterServices();
        }

        private static void MigrateDatabase()
        {
            //var dbContext = (RepositoryContext)_services.BuildServiceProvider().GetService(typeof(RepositoryContext));
            //dbContext.Database.Migrate();
        }

        private static void RegisterServices()
        {
            string connection = _configuration.GetConnectionString("DefaultConnection");

            _services.AddTransient<IRepositoryContextFactory, RepositoryContextFactory>();
            _services.AddTransient<ICurrencyRepository>(provider => new CurrencyRepository(connection, provider.GetService<IRepositoryContextFactory>(), provider.GetService<IOptions<CurrencyOptions>>()));
            _services.AddTransient<ILastUpdateRepository>(provider => new LastUpdateRepository(connection, provider.GetService<IRepositoryContextFactory>()));
            _services.AddScoped<IRateRepository>(provider => new RateRepository(connection, provider.GetService<IRepositoryContextFactory>()));
        }
    }
}
