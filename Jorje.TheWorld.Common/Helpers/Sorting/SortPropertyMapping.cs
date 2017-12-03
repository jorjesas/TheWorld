using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Common.Helpers.Sorting
{
    public class SortPropertyMapping<TSource, TDestination> : ISortPropertyMapping
    {
        public Dictionary<string, SortPropertyMappingValue> _mappingDictionary { get; private set; }
        public SortPropertyMapping(Dictionary<string, SortPropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }
    }
}
