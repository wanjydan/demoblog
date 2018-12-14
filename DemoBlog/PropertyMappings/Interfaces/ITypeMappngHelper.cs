using System;

namespace DemoBlog.Mappings.Interfaces
{
    public interface ITypeHelperService
    {
        Tuple<bool, string[]> TypeHasProperties<T>(string fields);
    }
}