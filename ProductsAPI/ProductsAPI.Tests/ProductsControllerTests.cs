using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Controllers;
using ProductsAPI.Models;
using ProductsAPI.Services;
using Xunit;

namespace ProductsAPI.Tests
{

    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _productController;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _productController = new ProductsController(null, _mockProductService.Object);
        }

        // Test: Get All Products

        [Fact]
        public async Task GetProducts_ReturnsListOfProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, SKU = "ABC123", Title = "Product 1", BuyerId = "49ec2a8703224eea9dec16b22546477e", Active = true },
                new Product { Id = 2, SKU = "XYZ789", Title = "Product 2", BuyerId = "a790a7b6bf2a48569066c46306c3332d", Active = false }
            };

            _mockProductService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            var result = await _productController.GetProducts();

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsType<List<Product>>(actionResult.Value);
            Assert.Equal(2, returnedProducts.Count);
        }

        // Test: Get Product by ID - Found

        [Fact]
        public async Task GetProduct_ExistingId_ReturnsProduct()
        {
            var product = new Product { Id = 1, SKU = "ABC123", Title = "Product 1", BuyerId = "49ec2a8703224eea9dec16b22546477e", Active = true };

            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            var result = await _productController.GetProduct(1);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal("ABC123", returnedProduct.SKU);
        }

        // Test: Get Product by ID - Not Found

        [Fact]
        public async Task GetProduct_NonExistingId_ReturnsNotFound()
        {
            _mockProductService.Setup(service => service.GetProductByIdAsync(99)).ReturnsAsync((Product)null);

            var result = await _productController.GetProduct(99);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test: Create Product - Success

        [Fact]
        public async Task CreateProduct_ValidProduct_ReturnsCreatedProduct()
        {
            var newProduct = new Product { Id = 3, SKU = "NEW456", Title = "New Product", BuyerId = "49ec2a8703224eea9dec16b22546477e", Active = true };

            _mockProductService.Setup(service => service.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(newProduct);

            var result = await _productController.CreateProduct(newProduct);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdProduct = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal("NEW456", createdProduct.SKU);
        }

        // Test: Create Product - Invalid Input

        [Fact]
        public async Task CreateProduct_InvalidProduct_ReturnsBadRequest()
        {
            Product invalidProduct = null;

            var result = await _productController.CreateProduct(invalidProduct);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // Test: Update Product - Success

        [Fact]
        public async Task UpdateProduct_ExistingId_ReturnsUpdatedProduct()
        {
            var updatedProduct = new Product { Id = 1, SKU = "ABC123", Title = "Updated Product", BuyerId = "49ec2a8703224eea9dec16b22546477e", Active = false };

            _mockProductService.Setup(service => service.UpdateProductAsync(1, It.IsAny<Product>())).ReturnsAsync(updatedProduct);

            var result = await _productController.UpdateProduct(1, updatedProduct);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal("Updated Product", returnedProduct.Title);
        }

        // Test: Update Product - Not Found

        [Fact]
        public async Task UpdateProduct_NonExistingId_ReturnsNotFound()
        {
            _mockProductService.Setup(service => service.UpdateProductAsync(99, It.IsAny<Product>())).ReturnsAsync((Product)null);

            var result = await _productController.UpdateProduct(99, new Product());

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test: Delete Product - Success

        [Fact]
        public async Task DeleteProduct_ExistingId_ReturnsNoContent()
        {
            _mockProductService.Setup(service => service.DeleteProductAsync(1)).ReturnsAsync(true);

            var result = await _productController.DeleteProduct(1);

            Assert.IsType<NoContentResult>(result);
        }

        // Test: Delete Product - Not Found

        [Fact]
        public async Task DeleteProduct_NonExistingId_ReturnsNotFound()
        {
            _mockProductService.Setup(service => service.DeleteProductAsync(99)).ReturnsAsync(false);

            var result = await _productController.DeleteProduct(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
