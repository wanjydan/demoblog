﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBlog.Services.Interfaces
{
    public interface ITypeHelperService
    {
        Tuple<bool, string[]> TypeHasProperties<T>(string fields);
    }
}