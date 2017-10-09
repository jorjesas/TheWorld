using Jorje.TheWorld.Domain;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IStopRepository : IAbstractRepository<Stop>
    {
        Task<Stop> GetStopById(int stopId);
        Task<Stop> CreateStop(Stop stop);
        Task<Stop> DeleteStop(int stopId);
        Task<Stop> UpdateStop(Stop stop);
    }
}
