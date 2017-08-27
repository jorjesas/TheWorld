using Jorje.TheWorld.Bll.IBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using Jorje.TheWorld.Models;
using System.Threading.Tasks;
using Jorje.TheWorld.Dal.IRepositories;

namespace Jorje.TheWorld.Bll.Business
{
    public class StopBus : IStopBus
    {
        IStopRepository _stopRepo;

        #region ctor
        public StopBus(IStopRepository stopRepo)
        {
            _stopRepo = stopRepo;
        }
        #endregion

        public async Task<StopModel> GetStop(int stopId)
        {
            var stopDTO = await _stopRepo.GetStopById(stopId);

            if (stopDTO != null)
            {
                return new StopModel() { Name = stopDTO.Name, Arrival = stopDTO.Arrival };
            }

            return null;
        }

        public Task<StopModel> GetStopsByTrip(int tripId)
        {
            throw new NotImplementedException();
        }
    }
}
