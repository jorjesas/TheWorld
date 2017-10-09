using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.IBusiness
{
    public interface IStopBus
    {
        Task<StopDTO> GetStop(int stopId);
        Task<StopDTO> GetStopsByTrip(int tripId);
        Task<bool> CreateStop(StopDTO stopModel);
        Task<StopDTO> DeleteStop(int stopId);


    }
}
