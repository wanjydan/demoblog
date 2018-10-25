using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoBlog.Services.Interfaces;

namespace DemoBlog.Services
{
    public class PropertyMapping <TSourse, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; private set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }
    }
}
