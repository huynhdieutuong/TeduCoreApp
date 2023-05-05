using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;

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

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var productVm = _productService.GetById(id);
            return new OkObjectResult(productVm);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            _productService.Save();
            return new OkObjectResult(true);
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel productVm)
        {
            _productService.Add(productVm);
            _productService.Save();
            return new OkObjectResult(productVm);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProductModel productVm)
        {
            var result = _productService.Update(id, productVm);
            if (!result) return new BadRequestResult();

            _productService.Save();
            return new OkObjectResult(productVm);
        }
    }
}
