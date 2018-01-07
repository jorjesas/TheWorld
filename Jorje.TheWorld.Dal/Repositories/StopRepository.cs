using Jorje.TheWorld.Common.Helpers;
using Jorje.TheWorld.Common.Helpers.Extensions;
using Jorje.TheWorld.Common.Helpers.Pagination;
using Jorje.TheWorld.Dal.Context;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Dal.Sort;
using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
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
            _sortPropertyMappingService = new StopSortPropertyMappingService();
        }

        public async Task<PagedList<Stop>> GetStops(PaginationProperties paginationProperties)
        {
            var query = GetAll();
                            //.OrderBy(s => s.Name);

            if (!string.IsNullOrEmpty(paginationProperties.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = paginationProperties.SearchQuery
                    .Trim().ToLowerInvariant();

                query = query
                    .Where(s => s.Name.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || s.Description.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            //query = query.OrderBy(s => s.Name);

            query = query.ApplySort(paginationProperties.OrderBy,
                _sortPropertyMappingService.GetPropertyMapping<StopDTO, Stop>());

            return await PagedList<Stop>.Create(query, paginationProperties.PageNumber, paginationProperties.PageSize);
            //return await query.ToListAsync();
        }

        public async Task<IEnumerable<Stop>> GetStops(IEnumerable<int> ids)
        {
            var query = GetAll()
                .Where(s => ids.Contains(s.Id))
                .OrderBy(s => s.Latitude)
                .OrderBy(s => s.Longitude)
                .OrderBy(s => s.Name);

            return await query.ToListAsync();
        }

            public async Task<Stop> GetStopById(int stopId)
        {
            var query = GetAll().Where(m => m.Id == stopId);

            return await query.FirstOrDefaultAsync();
        }

        //public async Task<Stop> DeleteStop(int stopId)
        //{
        //    var query = GetAll().Where(m => m.Id == stopId);

        //    Stop stop =  await query.FirstOrDefaultAsync();

        //    if (stop != null)
        //    {
                

        //        Delete(stop);
        //        await SaveChanges();
        //    }

        //    return stop;
        //}

        public async Task<bool> CreateStop(Stop stop)
        {
            return await InsertEntity(stop);
        }

        public async Task<bool> UpdateStop(Stop stop)
        {
            return await UpdateEntity(stop);
        }

        public async Task<bool> DeleteStop(int id)
        {
            Stop stop = await GetStopById(id);

            return await DeleteEntity(stop);
        }
    }
}
