﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jorje.TheWorld.Common.Services.Contract
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
