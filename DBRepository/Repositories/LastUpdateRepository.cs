using System;
using DBRepository.Interfaces;
using System.Threading.Tasks;
using Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DBRepository.Repositories
{
    public class LastUpdateRepository : BaseRepository, ILastUpdateRepository
    {
        public LastUpdateRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory)
        {

        }

        public async Task Add(LastUpdate item)
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                await dbContext.LastUpdates.AddAsync(item);

                dbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LastUpdate>> GetAll()
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                return await dbContext.LastUpdates.ToListAsync();
            }
        }

        public async Task<LastUpdate> GetLastUpdate()
        {
            using (var dbContext = ContextFactory.CreateDBContext(ConnectionString))
            {
                return await dbContext.LastUpdates.OrderByDescending(x => x.LastUpdateStart).FirstOrDefaultAsync();
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(LastUpdate item)
        {
            throw new NotImplementedException();
        }
    }
}
