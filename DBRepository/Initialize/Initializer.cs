using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using DBRepository.Interfaces;
using DBRepository.Factories;
using DBRepository.Repositories;

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
            var dbContext = (RepositoryContext)_services.BuildServiceProvider().GetService(typeof(RepositoryContext));
            //dbContext.Database.Migrate();
        }

        private static void RegisterServices()
        {
            _services.AddTransient<IRepositoryContextFactory, RepositoryContextFactory>();
            _services.AddTransient<ICurrencyRepository>(provider => new CurrencyRepository(_configuration.GetConnectionString("DefaultConnection"), provider.GetService<IRepositoryContextFactory>()));
            _services.AddTransient<ILastUpdateRepository>(provider => new LastUpdateRepository(_configuration.GetConnectionString("DefaultConnection"), provider.GetService<IRepositoryContextFactory>()));
            _services.AddScoped<IRateRepository>(provider => new RateRepository(_configuration.GetConnectionString("DefaultConnection"), provider.GetService<IRepositoryContextFactory>()));
        }
    }
}
