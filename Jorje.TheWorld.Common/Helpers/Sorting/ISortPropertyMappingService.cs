using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Common.Helpers.Sorting
{
    public interface ISortPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);

        Dictionary<string, SortPropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
