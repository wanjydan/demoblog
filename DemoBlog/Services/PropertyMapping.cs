using System.Collections.Generic;
using DemoBlog.Services.Interfaces;

namespace DemoBlog.Services
{
    public class PropertyMapping<TSourse, TDestination> : IPropertyMapping
    {
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }

        public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; }
    }
}