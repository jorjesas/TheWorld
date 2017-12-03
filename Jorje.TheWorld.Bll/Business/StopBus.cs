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
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Jorje.TheWorld.Common.Helpers;
using Jorje.TheWorld.Common.Helpers.Pagination;
using Jorje.TheWorld.Common.Helpers.Sorting;
using Jorje.TheWorld.Dal.Sort;
using Jorje.TheWorld.Bll.Containers;
using Jorje.TheWorld.Common.Services.Contract;
using Jorje.TheWorld.Common.Services;

namespace Jorje.TheWorld.Bll.Business
{
    public class StopBus : IStopBus
    {
        IStopRepository _stopRepo;
        ISortPropertyMappingService _sortPropertyMappingService;
        ITypeHelperService _typeHelperService;

        #region ctor
        public StopBus(IStopRepository stopRepo)
        {
            _stopRepo = stopRepo;
            _sortPropertyMappingService = new StopSortPropertyMappingService();
            _typeHelperService = new TypeHelperService();
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

        public async Task<ResourceDataResult> GetStops(PaginationProperties paginationProperties)
        {
            ResourceDataResult result = new ResourceDataResult();

            if (!_sortPropertyMappingService.ValidMappingExistsFor<StopDTO, Stop>(paginationProperties.OrderBy))
            {
                result.StatusCode = 400;
                result.ErrorMessage = "Invalid sorting field in query string.";
                return result;
            }

            PagedList<Stop> stops = await _stopRepo.GetStops(paginationProperties);

            //PagedList<StopDTO> stopModels = new PagedList<StopDTO>(StopMapper.ConvertEntityToModel(stops), stops.Count, stops.CurrentPage, stops.PageSize);

            if (stops != null && stops.Count > 0)
            {
                result.Result = new PagedList<StopDTO>(StopMapper.ConvertEntityToModel(stops), stops.Count, stops.CurrentPage, stops.PageSize);
            }

            return result;
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

        public async Task<StopForUpdateDTO> GetUpdateModelForStop(int stopId)
        {
            var stop = await _stopRepo.GetStopById(stopId);

            if (stop == null)
            {
                return null;
            }

            StopForUpdateDTO updatedStop = StopMapper.ConvertEntityToModel<StopForUpdateDTO>(stop);

            return updatedStop;
        }

    }
}
