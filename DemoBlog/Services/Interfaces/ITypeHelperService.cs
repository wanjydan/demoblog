using System;

namespace DemoBlog.Services.Interfaces
{
    public interface ITypeHelperService
    {
        Tuple<bool, string[]> TypeHasProperties<T>(string fields);
    }
}