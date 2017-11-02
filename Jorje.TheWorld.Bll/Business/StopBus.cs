using Jorje.TheWorld.Bll.IBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using Jorje.TheWorld.Models;
using System.Threading.Tasks;
using Jorje.TheWorld.Dal.IRepositories;
using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Bll.Mappers;
using Microsoft.AspNetCore.JsonPatch;

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

        public async Task<StopDTO> GetStop(int stopId)
        {
            var stop = await _stopRepo.GetStopById(stopId);


            if (stop != null)
            {
                return StopMapper.ConvertEntityToModel(stop);
            }

            return null;
        }

        public Task<StopDTO> GetStopsByTrip(int tripId)
        {
            throw new NotImplementedException();
        }

        public async Task<StopDTO> CreateStop(StopForCreationDTO stopModel)
        {
            Stop stop = StopMapper.ConvertCreationModelToEntity(stopModel);
                
            if (await _stopRepo.CreateStop(stop))
            {
                return StopMapper.ConvertEntityToModel(stop);
            }

            return null;
        }

        public async Task<bool> DeleteStop(StopDTO stopModel)
        {
            //Stop stop = StopMapper.ConvertModelToEntity(stopModel);

            if (stopModel!= null && await _stopRepo.DeleteStop(stopModel.Id))
            {
                return true;
            }

            return false;
        }

        public async Task<StopDTO> UpdateStop(int stopId, StopForUpdateDTO stopModel)
        {
            var stop = await _stopRepo.GetStopById(stopId);

            //upsert
            if (stop == null)
            {
                stop = new Stop() { Id=stopId };
            }

            stop = StopMapper.UpdateEntityToModel(stop, stopModel);

            if (stop != null && await _stopRepo.UpdateStop(stop))
            {
                return StopMapper.ConvertEntityToModel(stop);
            }

            return null;
        }

        public async Task<StopDTO> PartialUpdateStop(int stopId, JsonPatchDocument<StopForUpdateDTO> patchDoc)
        {
            var stop = await _stopRepo.GetStopById(stopId);

            if (stop == null)
            {
                return null;
            }

            StopForUpdateDTO updatedStop = StopMapper.ConvertEntityToModel<StopForUpdateDTO>(stop);

            patchDoc.ApplyTo(updatedStop);

            StopMapper.UpdateModel<StopForUpdateDTO>(updatedStop, stop);

            if (stop != null && await _stopRepo.UpdateStop(stop))
            {
                return StopMapper.ConvertEntityToModel(stop);
            }

            return null;
        }

    }
}
