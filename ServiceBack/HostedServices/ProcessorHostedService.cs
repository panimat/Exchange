using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Linq;
using System.IO;
using Models;
using DBRepository.Interfaces;
using System.Net;
using Newtonsoft.Json;

namespace ServiceBack.HostedServices
{ 
    public class ProcessorHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<ProcessorHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;


        public ProcessorHostedService(ILogger<ProcessorHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(GetMoney, null, TimeSpan.Zero,
                TimeSpan.FromMinutes((double)JObject.Parse(
                    File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json")))
                    .SelectToken("Frequency").First.ToObject<JProperty>().Value));

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
            WebRequest request = WebRequest.Create("https://api.uphold.com/v0/ticker/" + pair);

            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = null;
            Stream dataStream = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                dataStream = response.GetResponseStream();

                if (response.StatusDescription == "OK")
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();

                    parseResponse(responseFromServer, pair);

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Problem with http request or response /n" + ex.Message);
            }
            finally
            {
                dataStream.Close();
                response.Close();
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