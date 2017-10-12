using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.Mappers
{
    public static class StopMapper
    {
        public static StopDTO ConvertEntityToModel(Stop stop)
        {
            return AutoMapperContainer.GenericConvert<StopDTO>(stop);
        }

        public static Stop ConvertCreationModelToEntity(StopForCreationDTO stopInput)
        {
            return AutoMapperContainer.GenericConvert<Stop>(stopInput);
        }

        public static Stop ConvertModelToEntity(StopDTO stopModel)
        {
            return AutoMapperContainer.GenericConvert<Stop>(stopModel);
        }

        public static Stop UpdateEntityToModel(Stop stop, StopForUpdateDTO stopInput)
        {
            stop.Description = stopInput.Description;
            stop.Name = stopInput.Name;
            stop.Latitude = stopInput.Latitude;
            stop.Longitude = stopInput.Longitude;

            return stop;
        }
    }
}
