using Jorje.TheWorld.Dal.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IStopRepository : IAbstractRepository<StopDTO>
    {
        Task<StopDTO> GetStopById(int stopId);
    }
}
