using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            productCategoryVm.SeoAlias = TextHelper.ToUnsignString(productCategoryVm.Name);

            var productCategory = _mapper.Map<ProductCategory>(productCategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productCategoryVm;
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            var productCategories = _productCategoryRepository.FindAll().OrderBy(x => x.ParentId);
            return _mapper.ProjectTo<ProductCategoryViewModel>(productCategories).ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return GetAll();
            }

            var productCategories = _productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword)).OrderBy(x => x.ParentId);
            return _mapper.ProjectTo<ProductCategoryViewModel>(productCategories).ToList();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            var productCategories = _productCategoryRepository.FindAll(x => x.Status == Status.Active && x.ParentId == parentId).OrderBy(x => x.ParentId);
            return _mapper.ProjectTo<ProductCategoryViewModel>(productCategories).ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            var productCategory = _productCategoryRepository.FindById(id);
            return _mapper.Map<ProductCategoryViewModel>(productCategory);
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var productCategories = _productCategoryRepository.FindAll(x => x.HomeFlag == true, c => c.Products).OrderBy(x => x.HomeOrder);
            return _mapper.ProjectTo<ProductCategoryViewModel>(productCategories).ToList();
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            productCategoryVm.SeoAlias = TextHelper.ToUnsignString(productCategoryVm.Name);

            var productCategory = _mapper.Map<ProductCategory>(productCategoryVm);
            _productCategoryRepository.Update(productCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _productCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }
    }
}
