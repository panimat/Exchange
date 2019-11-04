using System;
using System.Collections.Generic;
using DBRepository.Interfaces;
using System.Threading.Tasks;
using Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Options;


namespace DBRepository.Repositories
{
    public class CurrencyRepository : BaseRepository, ICurrencyRepository
    {
        public CurrencyRepository(string connectionString, IRepositoryContextFactory contextFactory, IOptions<CurrencyOptions> currencyOptions) : base(connectionString, contextFactory)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                try
                {
                    foreach (var item in currencyOptions.Value.Currency)                    
                        Add(new Currencies { Currency = item });
                    
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task Add(Currencies item)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                if (dbContext.Currencies.Where(x => x.Currency == item.Currency).Count() == 0)
                    await dbContext.Currencies.AddAsync(item);
            }
        }

        public async Task AddAllCurrencies(IEnumerable<Currencies> currencies)
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Currencies>> GetAll()
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                return await dbContext.Currencies.ToListAsync();
            }
        }
        
        public IEnumerable<string> GetPairs()
        {
            var currencies = this.GetAll().Result.ToList();

            var pairs = new List<string>();

            for (int i = 0; i < currencies.Count - 1; i++)
                for (int j = i + 1; j < currencies.Count; j++)
                {
                    pairs.Add(currencies[i].Currency + currencies[j].Currency);
                    pairs.Add(currencies[j].Currency + currencies[i].Currency);
                }

            return pairs;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Currencies item)
        {
            throw new NotImplementedException();
        }
    }
}
