using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeduCoreApp.Application.Interfaces;

namespace TeduCoreApp.WebApi.Controllers
{
    [Authorize]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(int? categoryId, string keyword, int page = 1, int pageSize = 10)
        {
            var productsPageResult = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(productsPageResult);
        }
    }
}
