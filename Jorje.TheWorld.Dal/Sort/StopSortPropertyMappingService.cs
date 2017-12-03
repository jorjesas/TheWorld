using Jorje.TheWorld.Common.Helpers.Sorting;
using Jorje.TheWorld.Domain;
using Jorje.TheWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Dal.Sort
{
    public class StopSortPropertyMappingService : SortPropertyMappingService
    {
        public StopSortPropertyMappingService() : base()
        {
            _sortPropertyMapping = new Dictionary<string, SortPropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                       {{ "Id", new SortPropertyMappingValue(new List<string>() { "Id" })},
                        { "Name", new SortPropertyMappingValue(new List<string>() { "Name" })}};

            _propertyMappings.Add(new SortPropertyMapping<StopDTO, Stop>(_sortPropertyMapping));
        }
    }
}
