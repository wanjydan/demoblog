using System;
using System.Collections.Generic;

namespace DemoBlog.Services.Interfaces
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        Tuple<bool, string[]> ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}