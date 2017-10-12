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
        Task<StopDTO> CreateStop(StopForCreationDTO stopModel);
        Task<bool> DeleteStop(StopDTO stopModel);
        Task<StopDTO> UpdateStop(int stopId, StopForUpdateDTO stopModel);


    }
}
