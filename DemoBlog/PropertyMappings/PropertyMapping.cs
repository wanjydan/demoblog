using System.Collections.Generic;
using DemoBlog.PropertyMappings.Interfaces;

namespace DemoBlog.PropertyMappings
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