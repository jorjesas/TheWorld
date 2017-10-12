using Jorje.TheWorld.Dal.Context;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.Repositories
{
    public class StopRepository : AbstractRepository<Stop>, IStopRepository
    {
        public StopRepository(WorldDBContext context) : base(context)
        {              
        }

        public async Task<Stop> GetStopById(int stopId)
        {
            var query = GetAll().Where(m => m.Id == stopId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Stop> DeleteStop(int stopId)
        {
            var query = GetAll().Where(m => m.Id == stopId);

            Stop stop =  await query.FirstOrDefaultAsync();

            if (stop != null)
            {
                Delete(stop);
                await SaveChanges();
            }

            return stop;
        }

        public async Task<bool> CreateStop(Stop stop)
        {
            return await InsertEntity(stop);
        }

        public async Task<bool> UpdateStop(Stop stop)
        {
            return await UpdateEntity(stop);
        }

        public async Task<bool> DeleteStop(Stop stop)
        {
            return await DeleteEntity(stop);
        }
    }
}
