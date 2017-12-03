using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
using System;
using System.Collections;
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

        public static List<StopDTO> ConvertEntityToModel(IEnumerable<Stop> stops)
        {
            var stopModelList = new List<StopDTO>();

            foreach (var stop in stops)
            {
                stopModelList.Add(AutoMapperContainer.GenericConvert<StopDTO>(stop));
            }
            return stopModelList;
        }

        public static Stop ConvertCreationModelToEntity(StopForCreationDTO stopInput)
        {
            return AutoMapperContainer.GenericConvert<Stop>(stopInput);
        }

        public static Stop ConvertUpdateModelToEntity(StopForUpdateDTO stopInput)
        {
            return AutoMapperContainer.GenericConvert<Stop>(stopInput);
        }

        public static T ConvertEntityToModel<T>(Stop stop)
        {
            return AutoMapperContainer.GenericConvert<T>(stop);
        }

        public static Stop ConvertModelToEntity<T>(T model)
        {
            return AutoMapperContainer.GenericConvert<Stop>(model);
        }

        public static Stop UpdateModel<T>(T model, Stop stop)
        {
            return AutoMapperContainer.GenericConvert<T,Stop>(model, stop);
        }

        public static Stop ConvertModelToEntity(StopDTO stopModel)
        {
            return AutoMapperContainer.GenericConvert<Stop>(stopModel);
        }

        public static Stop UpdateEntityToModel(Stop stop, StopForUpdateDTO stopInput, bool isPartialUpdate = false)
        {
            stop.Description = stopInput.Description;
            stop.Name = stopInput.Name;
            stop.Latitude = stopInput.Latitude;
            stop.Longitude = stopInput.Longitude;

            return stop;
        }
    }
}
