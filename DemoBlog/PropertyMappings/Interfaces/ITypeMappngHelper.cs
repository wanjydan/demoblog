using System;

namespace DemoBlog.PropertyMappings.Interfaces
{
    public interface ITypeMappingHelper
    {
        Tuple<bool, string[]> TypeHasProperties<T>(string fields);
    }
}