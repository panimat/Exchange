using DBRepository.Interfaces;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DBRepository.Repositories
{
    public class RateRepository : BaseRepository, IRateRepository
    {
        public RateRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory)
        {

        }

        public async Task<IEnumerable<Rate>> GetAll()
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                try
                {
                    return await dbContext.Rates.Where(x => x.DateUpdate == dbContext.LastUpdates.OrderByDescending(y => y.LastUpdateStart).First().LastUpdateStart).ToListAsync();
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<Rate> GetByName(string pair)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                try
                {
                    return await dbContext.Rates.Where(x => x.DateUpdate == dbContext.LastUpdates.OrderByDescending(y => y.LastUpdateStart).First().LastUpdateStart && x.Pair == pair).FirstOrDefaultAsync();
                }
                catch
                {
                    return null;
                }
            }
        }

        public double GetAsk(string pair)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                try
                {
                    var rate = dbContext.Rates.Where(x => x.DateUpdate == dbContext.LastUpdates
                                    .OrderByDescending(y => y.LastUpdateStart)
                                    .First().LastUpdateStart && x.Pair == pair)
                                    .FirstOrDefault();

                    return rate.Ask;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public double GetBid(string pair)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                try
                {
                    var rate = dbContext.Rates.Where(x => x.DateUpdate == dbContext.LastUpdates
                                    .OrderByDescending(y => y.LastUpdateStart)
                                    .First().LastUpdateStart && x.Pair == pair)
                                    .FirstOrDefault();

                    return rate.Bid;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public async Task Add(Rate rate)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                try
                {
                    await dbContext.Rates.AddAsync(rate);

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    var abc = ex.Message;
                }
            }
        }

        public void Save()
        {
            
        }

        public void Update(Rate rate)
        {

        }

        public void Dispose()
        {

        }
    }
}


