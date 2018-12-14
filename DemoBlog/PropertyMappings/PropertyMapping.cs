using System.Collections.Generic;
using DemoBlog.Mappings.Interfaces;

namespace DemoBlog.Mappings
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