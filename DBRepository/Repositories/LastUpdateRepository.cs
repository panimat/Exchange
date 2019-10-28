using System;
using DBRepository.Interfaces;
using System.Threading.Tasks;
using Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DBRepository.Repositories
{
    public class LastUpdateRepository : BaseRepository, ILastUpdateRepository
    {
        public LastUpdateRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory)
        {

        }

        public async Task SetLastUpdate()
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                await dbContext.LastUpdates.AddAsync(new LastUpdate() { LastUpdateStart = DateTime.Now });

                dbContext.SaveChanges();
            }
        }

        public async Task<LastUpdate> GetLastUpdate()
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                return await dbContext.LastUpdates.OrderByDescending(x => x.LastUpdateStart).FirstOrDefaultAsync();
            }
        }
    }
}
