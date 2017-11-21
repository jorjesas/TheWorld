using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.Mappers
{
    public static class TripMapper
    {
        public static TripDTO ConvertEntityToModel(Trip trip)
        {
            return GenericMapper.ConvertEntityToModel<Trip, TripDTO>(trip);
        }

        public static Trip ConvertModelToEntity(TripDTO tripModel)
        {
            return GenericMapper.ConvertModelToEntity<Trip, TripDTO>(tripModel);
        }

        public static Trip UpdateEntityFromModel(Trip trip, TripDTO tripModel)
        {
            return GenericMapper.UpdateEntityFromModel<Trip, TripDTO>(trip, tripModel);
        }


    }
}
