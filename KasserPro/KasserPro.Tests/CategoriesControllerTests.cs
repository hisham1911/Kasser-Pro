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
    public class CategoriesControllerTests : IDisposable
    {
        private readonly KasserDbContext _context;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            var options = new DbContextOptionsBuilder<KasserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new KasserDbContext(options);
            _controller = new CategoriesController(_context);

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

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "مشروبات", Color = "#3B82F6", Icon = "🥤", StoreId = 1 },
                new Category { Id = 2, Name = "وجبات", Color = "#EF4444", Icon = "🍔", StoreId = 1 },
                new Category { Id = 3, Name = "تصنيف متجر آخر", StoreId = 2 }
            };
            _context.Categories.AddRange(categories);

            // Add some products to test counts
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "كولا", Price = 15m, CategoryId = 1, StoreId = 1 },
                new Product { Id = 2, Name = "عصير", Price = 20m, CategoryId = 1, StoreId = 1 }
            };
            _context.Products.AddRange(products);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCategories_ReturnsOnlyStoreCategories()
        {
            // Act
            var result = await _controller.GetCategories();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var categories = okResult?.Value as IEnumerable<object>;
            
            // Should return only 2 categories (belonging to store 1)
            categories.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetCategory_WithValidId_ReturnsCategory()
        {
            // Act
            var result = await _controller.GetCategory(1);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.Name.Should().Be("مشروبات");
        }

        [Fact]
        public async Task GetCategory_FromDifferentStore_ReturnsNotFound()
        {
            // Act - Try to get category from store 2
            var result = await _controller.GetCategory(3);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetCategoryProducts_ReturnsProductsInCategory()
        {
            // Act
            var result = await _controller.GetCategoryProducts(1);

            // Assert
            var products = result.Value;
            products.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateCategory_WithValidData_ReturnsCreatedCategory()
        {
            // Arrange
            var newCategory = new Category
            {
                Name = "حلويات",
                Color = "#F59E0B",
                Icon = "🍰"
            };

            // Act
            var result = await _controller.CreateCategory(newCategory);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            var category = createdResult?.Value as Category;
            category!.Name.Should().Be("حلويات");
            category.StoreId.Should().Be(1);
        }

        [Fact]
        public async Task CreateCategory_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var newCategory = new Category
            {
                Name = "مشروبات", // Already exists
                Color = "#000000"
            };

            // Act
            var result = await _controller.CreateCategory(newCategory);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteCategory_WithNoProducts_DeletesCategory()
        {
            // Arrange - Add an empty category
            var emptyCategory = new Category { Id = 10, Name = "تصنيف فارغ", StoreId = 1 };
            _context.Categories.Add(emptyCategory);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteCategory(10);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteCategory_WithProducts_ReturnsBadRequest()
        {
            // Act - Try to delete category 1 which has products
            var result = await _controller.DeleteCategory(1);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
