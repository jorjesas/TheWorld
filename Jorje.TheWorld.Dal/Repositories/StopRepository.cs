using Jorje.TheWorld.Dal.Context;
using Jorje.TheWorld.Dal.DomainModels;
using Jorje.TheWorld.Dal.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.Repositories
{
    public class StopRepository : AbstractRepository<StopDTO>, IStopRepository
    {
        public StopRepository(WorldDBContext context) : base(context)
        {
                
        }

        public async Task<StopDTO> GetStopById(int stopId)
        {
            var query = GetAll().Where(m => m.StopId == stopId);

            return await query.FirstAsync();
        }
    }
}
