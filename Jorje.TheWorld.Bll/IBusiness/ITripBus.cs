using Jorje.TheWorld.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        Task<TripDTO> CreateTrip(TripDTO tripModel);
        Task<bool> DeleteTrip(TripDTO tripModel);
        Task<TripDTO> UpdateTrip(int tripId, TripDTO tripModel);
        Task<TripDTO> PartialUpdateTrip(int stopId, JsonPatchDocument<TripDTO> patchDoc);
    }
}
