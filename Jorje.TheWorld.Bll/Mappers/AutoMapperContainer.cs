using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.Mappers
{
    public static class AutoMapperContainer
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                //Entities to Models
                cfg.CreateMap<Stop, StopDTO>();
                cfg.CreateMap<Stop, StopForUpdateDTO>();
                cfg.CreateMap<Trip, TripDTO>();
                cfg.CreateMap<TripStop, TripStopDTO>();
                cfg.CreateMap<PersonTrip, PersonTripDTO>();

                //Models to Entities
                cfg.CreateMap<StopDTO, Stop>();
                cfg.CreateMap<StopForCreationDTO, Stop>();
                cfg.CreateMap<StopForUpdateDTO, Stop>();
                cfg.CreateMap<TripDTO, Trip>();
                cfg.CreateMap<TripStopDTO, TripStop>();
                cfg.CreateMap<PersonTripDTO, PersonTrip>();
            });
        }

        public static T GenericConvert<T>(Object obj)
        {
            return AutoMapper.Mapper.Map<T>(obj);
        }

        public static T2 GenericConvert<T1,T2>(T1 source, T2 destination)
        {
            return AutoMapper.Mapper.Map(source, destination);
        }

        public static object GenericUpdate(Object obj, Object updatedObj, Type objType, Type updatedObjType)
        {
            return AutoMapper.Mapper.Map(obj, updatedObj, objType, updatedObjType);

        }

    }
}
