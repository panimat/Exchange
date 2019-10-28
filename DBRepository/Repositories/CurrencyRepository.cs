using System;
using System.Collections.Generic;
using DBRepository.Interfaces;
using System.Threading.Tasks;
using Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace DBRepository.Repositories
{
    public class CurrencyRepository : BaseRepository, ICurrencyRepository
    {
        public CurrencyRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                try
                {
                    var currencies = JObject.Parse(
                            File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json")))
                            .SelectToken("Currencies");

                    foreach (var item in currencies)
                    {
                        var val = item.ToObject<JProperty>().Value.ToString();

                        if (dbContext.Currencies.Where(x => x.Currency == val).Count() == 0)
                            dbContext.Currencies.Add(new Currencies { Currency = val });
                    }

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task SetCurrencies(IEnumerable<Currencies> currencies)
        {
            foreach (var item in currencies)
            {
                using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
                {
                    if (dbContext.Currencies.Where(x => x.Currency == item.Currency).FirstOrDefault() == null)                    
                        await dbContext.Currencies.AddAsync(item);

                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Currencies>> GetCurrencies()
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                return await dbContext.Currencies.ToListAsync();
            }
        }
        
        public IEnumerable<string> GetPairs()
        {
            var currencies = this.GetCurrencies().Result.ToList();

            var pairs = new List<string>();

            for (int i = 0; i < currencies.Count - 1; i++)
                for (int j = i + 1; j < currencies.Count; j++)
                {
                    pairs.Add(currencies[i].Currency + currencies[j].Currency);
                    pairs.Add(currencies[j].Currency + currencies[i].Currency);
                }

            return pairs;
        }
    }
}
