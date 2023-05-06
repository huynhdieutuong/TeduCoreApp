using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TeduCoreApp.Application.Implementation;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Helpers;
using Xunit;

namespace TeduCoreApp.Application.Test.Implementation
{
    public class ProductServiceTest
    {
        private readonly ProductService productService;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IRepository<Tag, string>> _mockTagRepository;
        private readonly Mock<IRepository<ProductTag, int>> _mockProductTagRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;

        public ProductServiceTest()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockTagRepository = new Mock<IRepository<Tag, string>>();
            _mockProductTagRepository = new Mock<IRepository<ProductTag, int>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            productService = new ProductService(
                _mockProductRepository.Object,
                _mockTagRepository.Object,
                _mockProductTagRepository.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object);
        }

        [Fact]
        public void Add_ValidData_ReturnSuccess()
        {
            //Arrange
            var productVm = EmbeddedJsonFileHelper.GetContent<ProductViewModel>(@"MockData\ProductService\Mock_ProductVm");
            var product = EmbeddedJsonFileHelper.GetContent<Product>(@"MockData\ProductService\Mock_Product");
            var listTag = EmbeddedJsonFileHelper.GetContent<List<Tag>>(@"MockData\ProductService\Mock_ListTag");

            _mockTagRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Tag, object>>>())).Returns(listTag.AsQueryable());
            _mockTagRepository.Setup(x => x.Add(It.IsAny<Tag>()));
            _mockMapper.Setup(x => x.Map<Product>(productVm)).Returns(product);
            _mockProductRepository.Setup(x => x.Add(It.IsAny<Product>()));

            //Act
            var result = productService.Add(productVm);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAll_ValidQuery_ReturnListProductVm()
        {
            //Arrange
            var listProduct = EmbeddedJsonFileHelper.GetContent<List<Product>>(@"MockData\ProductService\Mock_ListProduct");
            var listProductVm = EmbeddedJsonFileHelper.GetContent<List<ProductViewModel>>(@"MockData\ProductService\Mock_ListProductVm");

            _mockProductRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Product, object>>>())).Returns(listProduct.AsQueryable());
            _mockMapper.Setup(x => x.ProjectTo<ProductViewModel>(listProduct.AsQueryable(), null, It.IsAny<Expression<Func<ProductViewModel, object>>[]>())).Returns(listProductVm.AsQueryable());

            //Act
            var result = productService.GetAll();

            //Assert
            Assert.Equal(listProductVm.Count, result.Count);
        }

        [Fact]
        public void GetAllPaging_ValidQuery_ReturnPagedResult()
        {
            //Arrange
            var listProduct = EmbeddedJsonFileHelper.GetContent<List<Product>>(@"MockData\ProductService\Mock_ListProduct");
            var listProductVm = EmbeddedJsonFileHelper.GetContent<List<ProductViewModel>>(@"MockData\ProductService\Mock_ListProductVm");

            _mockProductRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Product, bool>>>())).Returns(listProduct.AsQueryable());
            _mockMapper.Setup(x => x.ProjectTo<ProductViewModel>(listProduct.AsQueryable(), null, It.IsAny<Expression<Func<ProductViewModel, object>>[]>())).Returns(listProductVm.AsQueryable());

            //Act
            var result = productService.GetAllPaging(5, "shirt", 1, 10);

            //Assert
            Assert.Equal(listProductVm.Count, result.Results.Count);
        }

        [Fact]
        public void GetById_ValidQuery_ReturnProductVm()
        {
            //Arrange
            var product = EmbeddedJsonFileHelper.GetContent<Product>(@"MockData\ProductService\Mock_Product");
            var productVm = EmbeddedJsonFileHelper.GetContent<ProductViewModel>(@"MockData\ProductService\Mock_ProductVm");

            _mockProductRepository.Setup(x => x.FindById(It.IsAny<int>(), It.IsAny<Expression<Func<Product, object>>>())).Returns(product);
            _mockMapper.Setup(x => x.Map<ProductViewModel>(product)).Returns(productVm);

            //Act
            var result = productService.GetById(5);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Update_ValidData_ReturnSuccess()
        {
            //Arrange
            var product = EmbeddedJsonFileHelper.GetContent<Product>(@"MockData\ProductService\Mock_Product");
            var productVm = EmbeddedJsonFileHelper.GetContent<UpdateProductModel>(@"MockData\ProductService\Mock_ProductVm");
            var listTag = EmbeddedJsonFileHelper.GetContent<List<Tag>>(@"MockData\ProductService\Mock_ListTag");

            _mockProductRepository.Setup(x => x.FindById(It.IsAny<int>())).Returns(product);
            _mockTagRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Tag, object>>>())).Returns(listTag.AsQueryable());
            _mockTagRepository.Setup(x => x.Add(It.IsAny<Tag>()));
            _mockProductRepository.Setup(x => x.Update(product));

            //Act
            var result = productService.Update(5, productVm);

            //Assert
            Assert.True(result);
        }
    }
}
