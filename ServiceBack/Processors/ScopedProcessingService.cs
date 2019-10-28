using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceBack.Interfaces;


using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Models;

namespace ServiceBack.Processors
{
    internal class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger _logger;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task WriteDataRate(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ParsePair();
                //testRequest("USDEUR");

                await Task.Delay(10000, stoppingToken);
            }
        }


        /*
        private void testDb(Rate rate)
        {
            db.Rates.Add(rate);

            db.SaveChanges();
        }

        private void testRequest(string pair)
        {
            WebRequest request = WebRequest.Create("https://api.uphold.com/v0/ticker/" + pair);

            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();

            if (response.StatusDescription == "OK")
            {
                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();
                //Console.WriteLine(responseFromServer);
                testParse(responseFromServer);

                reader.Close();
            }

            dataStream.Close();
            response.Close();
        }
        
        private void testParse(string jsonExample)
        {
            JArray answer = JArray.Parse(jsonExample);

            var lastUpdate = new LastUpdate();
            lastUpdate.LastUpdateStart = DateTime.Now;

            db.LastUpdates.Add(lastUpdate);

            foreach (JObject obj in answer)
            {
                foreach (KeyValuePair<String, JToken> app in obj)
                {
                    Rate rate = new Rate();

                    if (app.Value["code"] == null)
                    {
                        rate.ask = (decimal)app.Value["ask"];
                        rate.bid = (decimal)app.Value["bid"];
                        rate.currency = (string)app.Value["currency"];
                    }

                    rate.DateUpdate = lastUpdate;
                    testDb(rate);
                }
            }
        }
        */

        private void ParsePair()
        {
            JToken currencyJson = JToken.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json")));

            foreach (var obj in currencyJson)
            {

                /*
                if ()
                foreach (var app in obj)
                {
                    Rate rate = new Rate();

                    if (app.Value["code"] == null)
                    {
                        rate.ask = (decimal)app.Value["ask"];
                        rate.bid = (decimal)app.Value["bid"];
                        rate.currency = (string)app.Value["currency"];
                    }

                    rate.DateUpdate = lastUpdate;
                    testDb(rate);
                }
                */
            }
        }
    }
}
