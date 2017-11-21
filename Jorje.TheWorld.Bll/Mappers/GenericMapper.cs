using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Bll.Mappers
{
    public static class GenericMapper
    {
        public static TModel ConvertEntityToModel<TEntity, TModel>(TEntity entity)
        {
            return AutoMapperContainer.GenericConvert<TModel>(entity);
        }

        public static TEntity ConvertModelToEntity<TEntity, TModel>(TModel model)
        {
            return AutoMapperContainer.GenericConvert<TEntity>(model);
        }

        public static TEntity UpdateEntityFromModel<TEntity, TModel>(TEntity entity, TModel model)
        {
            return AutoMapperContainer.GenericConvert<TModel, TEntity>(model, entity);
        }
    }
}
