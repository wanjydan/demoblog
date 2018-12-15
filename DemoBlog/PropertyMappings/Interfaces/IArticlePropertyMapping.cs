using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBlog.PropertyMappings.Interfaces
{
    public interface IArticlePropertyMapping
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        Tuple<bool, string[]> ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}
