using Jorje.TheWorld.Bll.Containers;
using Jorje.TheWorld.Common.Helpers;
using Jorje.TheWorld.Common.Helpers.Pagination;
using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.IBusiness
{
    public interface IStopBus
    {
        Task<StopDTO> GetStop(int stopId);
        Task<ResourceDataResult> GetStops(PaginationProperties paginationProperties);
        Task<IEnumerable<StopDTO>> GetStops(IEnumerable<int> ids);
        Task<StopDTO> GetStopsByTrip(int tripId);
        Task<StopDTO> CreateStop(StopForCreationDTO stopModel);
        Task<bool> DeleteStop(StopDTO stopModel);
        Task<ResourceDataResult> UpdateStop(int stopId, StopForUpdateDTO stopModel);
        Task<StopForUpdateDTO> GetUpdateModelForStop(int stopId);


    }
}
