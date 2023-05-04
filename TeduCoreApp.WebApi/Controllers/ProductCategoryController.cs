using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.WebApi.Controllers
{
    [Authorize]
    public class ProductCategoryController : BaseApiController
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly ILogger<ProductCategoryController> _logger;

        public ProductCategoryController(
            IProductCategoryService productCategoryService,
            ILogger<ProductCategoryController> logger)
        {
            _productCategoryService = productCategoryService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var productCategoriesVm = _productCategoryService.GetAll();
            return new OkObjectResult(productCategoriesVm);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var productCategoriesVm = _productCategoryService.GetById(id);
            return new OkObjectResult(productCategoriesVm);
        }

        [HttpPost]
        public IActionResult Add(ProductCategoryViewModel productCatVm)
        {
            var productCategoriesVm = _productCategoryService.Add(productCatVm);
            _productCategoryService.Save();

            return new OkObjectResult(productCategoriesVm);
        }

        [HttpPut("{id}")]
        public IActionResult Update(ProductCategoryViewModel productCatVm)
        {
            _productCategoryService.Update(productCatVm);
            _productCategoryService.Save();

            return new OkObjectResult(true);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productCategoryService.Delete(id);
            _productCategoryService.Save();

            return new OkObjectResult(true);
        }

        [HttpPut("ChangeOrder")]
        public IActionResult ChangeOrder(int sourceId, int targetId)
        {
            if (sourceId == targetId)
                return new BadRequestResult();

            _productCategoryService.ReOrder(sourceId, targetId);
            _productCategoryService.Save();

            return new OkObjectResult(true);
        }

        [HttpPut("UpdateParent")]
        public IActionResult UpdateParent(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (sourceId == targetId)
                return new BadRequestResult();

            _productCategoryService.UpdateParentId(sourceId, targetId, items);
            _productCategoryService.Save();

            return new OkObjectResult(true);
        }
    }
}
