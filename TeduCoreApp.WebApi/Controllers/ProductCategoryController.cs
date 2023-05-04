using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeduCoreApp.Application.Interfaces;

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
    }
}
