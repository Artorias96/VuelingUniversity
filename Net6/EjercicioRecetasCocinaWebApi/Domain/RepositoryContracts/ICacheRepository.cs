﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.RepositoryContracts
{
    public interface ICacheRepository
    {
        T GetCache<T>(string str);
        void SetCache<T>(string str, T generic);
    }
}
