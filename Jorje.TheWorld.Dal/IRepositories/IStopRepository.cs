using Jorje.TheWorld.Common.Helpers;
using Jorje.TheWorld.Common.Helpers.Pagination;
using Jorje.TheWorld.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IStopRepository : IAbstractRepository<Stop>
    {
        Task<PagedList<Stop>> GetStops(PaginationProperties paginationProperties);
        Task<Stop> GetStopById(int stopId);
        Task<bool> CreateStop(Stop stop);
        Task<bool> DeleteStop(int id);
        Task<bool> UpdateStop(Stop stop);
    }
}
