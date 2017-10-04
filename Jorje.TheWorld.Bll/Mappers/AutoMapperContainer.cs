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
                cfg.CreateMap<Stop, StopModel>();
            });
        }

        public static StopModel ConvertStopToStopModel(Stop stop)
        {
            return AutoMapper.Mapper.Map<StopModel>(stop);
        }

        public static Stop ConvertStopModelToStop(StopModel stopModel)
        {
            return AutoMapper.Mapper.Map<Stop>(stopModel);
        }
    }
}
