using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Models;
using DBRepository.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using Options;

namespace ServiceBack.HostedServices
{ 
    public class ProcessorHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<ProcessorHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<UpdateOptions> _updateOptions;
        private Timer _timer;


        public ProcessorHostedService(ILogger<ProcessorHostedService> logger, IServiceScopeFactory scopeFactory, IOptions<UpdateOptions> optionsUpdate)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _updateOptions = optionsUpdate;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(GetMoney, null, TimeSpan.Zero, TimeSpan.FromMinutes(_updateOptions.Value.FrequencyUpdateDB));

            return Task.CompletedTask;
        }

        private void GetMoney(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var serviceProvider = scope.ServiceProvider;
                    var lastUpdateRepository = (ILastUpdateRepository)serviceProvider.GetService(typeof(ILastUpdateRepository));
                    var currencyRepository = (ICurrencyRepository)serviceProvider.GetService(typeof(ICurrencyRepository));

                    lastUpdateRepository.SetLastUpdate();
                    
                    foreach (var item in currencyRepository.GetPairs())
                    {
                        testRequest(item);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }
            }
        }

        private void testRequest(string pair)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    //Get http for pair
                    string response = client.GetStringAsync("https://api.uphold.com/v0/ticker/" + pair).Result;

                    parseResponse(response, pair);

                }
                catch (HttpRequestException ex)
                {
                    _logger.LogInformation("Problem with http request or response /n" + ex.Message);
                }
            }
        }

        private void parseResponse(string jsonExample, string pair)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var serviceProvider = scope.ServiceProvider;
                    var lastUpdateRepository = (ILastUpdateRepository)serviceProvider.GetService(typeof(ILastUpdateRepository));
                    var rateRepository = (IRateRepository)serviceProvider.GetService(typeof(IRateRepository));

                    Rate rate = JsonConvert.DeserializeObject<Rate>(jsonExample);

                    rate.Pair = pair;
                    rate.DateUpdate = lastUpdateRepository.GetLastUpdate().Result.LastUpdateStart;

                    rateRepository.AddRate(rate);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Problem with adding rates to DB /n" + ex.Message);
                }
            }
        }
        

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}