using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Constants;
using TeduCoreApp.Utilities.Dtos;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<ProductTag, int> _productTagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IRepository<Tag, string> tagRepository, IRepository<ProductTag, int> productTagRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);

            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
                var product = _mapper.Map<Product>(productVm);
                foreach (var productTag in productTags)
                {
                    product.ProductTags.Add(productTag);
                }
                _productRepository.Add(product);

            }
            return productVm;
        }

        public void Delete(int id)
        {
            _productRepository.Remove(id);
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

        public ProductViewModel GetById(int id)
        {
            var product = _productRepository.FindById(id, x => x.ProductCategory);
            return _mapper.Map<ProductViewModel>(product);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public bool Update(int id, UpdateProductModel productVm)
        {
            var existsProduct = _productRepository.FindById(id);
            if (existsProduct == null) return false;

            if (existsProduct.Name != productVm.Name)
            {
                existsProduct.Name = productVm.Name;
                existsProduct.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
            }

            existsProduct.CategoryId = productVm.CategoryId;
            existsProduct.Image = productVm.Image;
            existsProduct.Price = productVm.Price;

            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }

                var existsProductTags = _productTagRepository.FindAll(x => x.ProductId == id).ToList();
                _productTagRepository.RemoveMultiple(existsProductTags);

                foreach (var productTag in productTags)
                {
                    existsProduct.ProductTags.Add(productTag);
                }
            }
            _productRepository.Update(existsProduct);
            return true;
        }
    }
}
