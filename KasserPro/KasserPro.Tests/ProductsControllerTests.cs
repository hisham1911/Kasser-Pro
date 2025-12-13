using KasserPro.Api.Controllers;
using KasserPro.Api.Data;
using KasserPro.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FluentAssertions;

namespace KasserPro.Tests
{
    public class ProductsControllerTests : IDisposable
    {
        private readonly KasserDbContext _context;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<KasserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new KasserDbContext(options);
            _controller = new ProductsController(_context);

            // Setup user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("StoreId", "1"),
                new Claim(ClaimTypes.Role, "Owner")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            SeedTestData();
        }

        private void SeedTestData()
        {
            var store = new Store { Id = 1, Name = "Test Store", IsActive = true };
            _context.Stores.Add(store);

            var category = new Category { Id = 1, Name = "مشروبات", Color = "#3B82F6", StoreId = 1 };
            _context.Categories.Add(category);

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "كولا", Price = 15.00m, Stock = 100, IsAvailable = true, CategoryId = 1, StoreId = 1 },
                new Product { Id = 2, Name = "عصير برتقال", Price = 20.00m, Stock = 50, IsAvailable = true, CategoryId = 1, StoreId = 1 },
                new Product { Id = 3, Name = "منتج متجر آخر", Price = 10.00m, Stock = 30, IsAvailable = true, StoreId = 2 }
            };
            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetProducts_ReturnsOnlyStoreProducts()
        {
            // Act
            var result = await _controller.GetProducts();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var products = okResult?.Value as IEnumerable<object>;
            
            // Should return only 2 products (belonging to store 1)
            products.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetProducts_FilterByCategory_ReturnsFilteredProducts()
        {
            // Act
            var result = await _controller.GetProducts(categoryId: 1);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var products = okResult?.Value as IEnumerable<object>;
            products.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetProducts_SearchByName_ReturnsMatchingProducts()
        {
            // Act
            var result = await _controller.GetProducts(search: "كولا");

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var products = okResult?.Value as IEnumerable<object>;
            products.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "منتج جديد",
                Price = 25.00m,
                Stock = 50,
                IsAvailable = true,
                CategoryId = 1
            };

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            var product = createdResult?.Value as Product;
            product!.Name.Should().Be("منتج جديد");
            product.StoreId.Should().Be(1); // Should be auto-set from claims
        }

        [Fact]
        public async Task CreateProduct_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "كولا", // Already exists
                Price = 25.00m,
                Stock = 50,
                IsAvailable = true
            };

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateProduct_WithZeroStock_SetsUnavailable()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "منتج بدون مخزون",
                Price = 25.00m,
                Stock = 0,
                IsAvailable = true // Will be set to false
            };

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            var product = createdResult?.Value as Product;
            product!.IsAvailable.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateStock_WithValidData_UpdatesStock()
        {
            // Arrange
            var newStock = 200;

            // Act
            var result = await _controller.UpdateStock(1, newStock);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            var product = await _context.Products.FindAsync(1);
            product!.Stock.Should().Be(200);
        }

        [Fact]
        public async Task UpdateStock_ToZero_SetsProductUnavailable()
        {
            // Act
            await _controller.UpdateStock(1, 0);

            // Assert
            var product = await _context.Products.FindAsync(1);
            product!.Stock.Should().Be(0);
            product.IsAvailable.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteProduct_WithNoOrders_DeletesProduct()
        {
            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            var product = await _context.Products.FindAsync(1);
            product.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
