using Jorje.TheWorld.Domain;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IStopRepository : IAbstractRepository<Stop>
    {
        Task<Stop> GetStopById(int stopId);
        Task<bool> CreateStop(Stop stop);
        Task<bool> DeleteStop(int id);
        Task<bool> UpdateStop(Stop stop);
    }
}
