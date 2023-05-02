using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.IRepositories;

namespace TeduCoreApp.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        private readonly IFunctionRepository _functionRepository;
        private readonly IMapper _mapper;

        public FunctionService(IFunctionRepository functionRepository, IMapper mapper)
        {
            _functionRepository = functionRepository;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<FunctionViewModel>> GetAllAsync()
        {
            var functions = _functionRepository.FindAll();

            return await _mapper.ProjectTo<FunctionViewModel>(functions).ToListAsync();
        }

        public List<FunctionViewModel> GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
