﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Common.Helpers.Sorting
{
    public abstract class SortPropertyMappingService : ISortPropertyMappingService
    {
        protected Dictionary<string, SortPropertyMappingValue> _sortPropertyMapping; 

           //new Dictionary<string, SortPropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           //{
           //    { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
           //    { "Genre", new PropertyMappingValue(new List<string>() { "Genre" } )},
           //    { "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },
           //    { "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
           //};

        protected IList<ISortPropertyMapping> _propertyMappings = new List<ISortPropertyMapping>();

        public SortPropertyMappingService()
        {
            //propertyMappings.Add(new SortPropertyMapping<AuthorDto, Author>(_sortPropertyMapping));
        }
        public Dictionary<string, SortPropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings.OfType<SortPropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;

        }

    }
}
