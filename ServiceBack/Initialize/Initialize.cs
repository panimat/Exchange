using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using DBRepository.Initialize;
using ServiceBack.HostedServices;

namespace ServiceBack.Initialize
{
    public static class Initialize
    {
        private static IServiceCollection _services;
        private static ILogger _logger;

        public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _services = services;
            logger.LogInformation("Configure business layer services...");
            RegisterServices();
            logger.LogInformation("Configure business layer services Done!");
            logger.LogInformation("Setup Entity Framework");
            services.AddDataServices(configuration, logger);
        }

        private static void RegisterServices()
        {
            _services.AddHostedService<ProcessorHostedService>();
        }

    }
}
