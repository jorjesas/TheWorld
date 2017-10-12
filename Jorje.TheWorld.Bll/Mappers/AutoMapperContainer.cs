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

                //Models to Entities
                cfg.CreateMap<StopDTO, Stop>();
                cfg.CreateMap<StopForCreationDTO, Stop>();
                cfg.CreateMap<StopForUpdateDTO, Stop>();
            });
        }

        public static T GenericConvert<T>(Object obj)
        {
            return AutoMapper.Mapper.Map<T>(obj);
        }

        public static object GenericUpdate(Object obj, Object updatedObj, Type objType, Type updatedObjType)
        {
            return AutoMapper.Mapper.Map(obj, updatedObj, objType, updatedObjType);
        }

    }
}
