using Jorje.TheWorld.Bll.IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jorje.TheWorld.Models;
using Jorje.TheWorld.Dal.IRepositories;

namespace Jorje.TheWorld.Bll.Business
{
    public class TripBus : ITripBus
    {
        public ITripRepository _tripRepository;

        public Task<IEnumerable<StopDTO>> GetStopsByTrip(int tripId)
        {
            throw new NotImplementedException();
        }

        public Task<TripDTO> GetTrip(int tripId)
        {
            throw new NotImplementedException();
        }
    }
}
