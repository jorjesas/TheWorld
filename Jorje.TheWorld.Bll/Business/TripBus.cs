using Jorje.TheWorld.Bll.IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jorje.TheWorld.Models;
using Jorje.TheWorld.Dal.IRepositories;
using Microsoft.AspNetCore.JsonPatch;
using Jorje.TheWorld.Bll.Mappers;
using Jorje.TheWorld.Domain;

namespace Jorje.TheWorld.Bll.Business
{
    public class TripBus : ITripBus
    {
        ITripRepository _tripRepo;

        #region ctor
        public TripBus(ITripRepository tripRepo)
        {
            _tripRepo = tripRepo;
        }
        #endregion

        public async Task<TripDTO> GetTrip(int tripId)
        {
            var trip = await _tripRepo.GetTripById(tripId);


            if (trip != null)
            {
                return TripMapper.ConvertEntityToModel(trip);
            }

            return null;
        }

        public async Task<TripDTO> CreateTrip(TripDTO tripModel)
        {
            Trip trip = TripMapper.ConvertModelToEntity(tripModel);

            if (await _tripRepo.CreateTrip(trip))
            {
                return TripMapper.ConvertEntityToModel(trip);
            }

            return null;
        }

        public async Task<bool> DeleteTrip(TripDTO tripModel)
        {
            if (tripModel != null && await _tripRepo.DeleteTrip(tripModel.Id))
            {
                return true;
            }

            return false;
        }

        public async Task<TripDTO> UpdateTrip(int tripId, TripDTO tripModel)
        {
            var trip = await _tripRepo.GetTripById(tripId);

            //upsert
            if (trip == null)
            {
                trip = new Trip() { Id = tripId };
            }

            trip = TripMapper.UpdateEntityFromModel(trip, tripModel);

            if (trip != null && await _tripRepo.UpdateTrip(trip))
            {
                return TripMapper.ConvertEntityToModel(trip);
            }

            return null;
        }

        public async Task<TripDTO> PartialUpdateTrip(int tripId, JsonPatchDocument<TripDTO> patchDoc)
        {
            var trip = await _tripRepo.GetTripById(tripId);

            if (trip == null)
            {
                return null;
            }

            TripDTO updatedTrip = TripMapper.ConvertEntityToModel(trip);

            patchDoc.ApplyTo(updatedTrip);

            trip = TripMapper.UpdateEntityFromModel(trip, updatedTrip);

            if (trip != null && await _tripRepo.UpdateTrip(trip))
            {
                return TripMapper.ConvertEntityToModel(trip);
            }

            return null;
        }

    }
}
