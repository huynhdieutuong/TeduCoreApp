using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            var products = _productRepository.FindAll(x => x.ProductCategory);
            return _mapper.ProjectTo<ProductViewModel>(products).ToList();
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            var totalRow = query.Count();

            query = query.OrderByDescending(x => x.DateCreated)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);

            var productsVm = _mapper.ProjectTo<ProductViewModel>(query).ToList();

            return new PagedResult<ProductViewModel>
            {
                Results = productsVm,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
    }
}
