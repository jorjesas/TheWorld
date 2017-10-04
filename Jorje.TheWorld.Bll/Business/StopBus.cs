using Jorje.TheWorld.Bll.IBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using Jorje.TheWorld.Models;
using System.Threading.Tasks;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Bll.Mappers;

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
            var stop = await _stopRepo.GetStopById(stopId);


            if (stop != null)
            {
                return AutoMapperContainer.ConvertStopToStopModel(stop);
            }

            return null;
        }

        public async Task<bool> CreateStop(StopModel stopModel)
        {
            Stop stop = AutoMapperContainer.ConvertStopModelToStop(stopModel);
                
            return await _stopRepo.CreateStop(stop);
        }

        public async Task<StopModel> DeleteStop(int stopId)
        {
            Stop stop = await _stopRepo.DeleteStop(stopId);

            if (stop != null)
            {
                return AutoMapperContainer.ConvertStopToStopModel(stop);
            }

            return null;
        }

        public Task<StopModel> GetStopsByTrip(int tripId)
        {
            throw new NotImplementedException();
        }
    }
}
