﻿using Jorje.TheWorld.Dal.Context;
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

            return await query.FirstAsync();
        }

        public async Task<Stop> DeleteStop(int stopId)
        {
            var query = GetAll().Where(m => m.Id == stopId);

            Stop stop =  await query.FirstAsync();

            if (stop != null)
            {
                Delete(stop);
                await SaveChanges();
            }

            return stop;
        }

        public async Task<bool> CreateStop(Stop stop)
        {
            if (stop != null)
            {
                Add(stop);
                await SaveChanges();
            }

            return true;
        }
    }
}
