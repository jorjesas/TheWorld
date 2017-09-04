using Jorje.TheWorld.Dal.DomainModels;
using Jorje.TheWorld.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.IRepositories
{
    public interface IStopRepository : IAbstractRepository<Stop>
    {
        Task<Stop> GetStopById(int stopId);
    }
}
