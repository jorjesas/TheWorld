using Jorje.TheWorld.Domain;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IStopRepository : IAbstractRepository<Stop>
    {
        Task<Stop> GetStopById(int stopId);
    }
}
