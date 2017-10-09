using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.IBusiness
{
    public interface ITripBus
    {
        Task<TripDTO> GetTrip(int tripId);
        Task<IEnumerable<StopDTO>> GetStopsByTrip(int tripId);
    }
}
