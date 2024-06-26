﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.ViewModels.System;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        Task<List<FunctionViewModel>> GetAllAsync();

        List<FunctionViewModel> GetAllByPermission(Guid userId);
    }
}
