using KasserPro.Api.Controllers;
using KasserPro.Api.Data;
using KasserPro.Api.Models;
using KasserPro.Api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FluentAssertions;

namespace KasserPro.Tests
{
    public class OrdersControllerTests : IDisposable
    {
        private readonly KasserDbContext _context;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            // Setup InMemory database
            var options = new DbContextOptionsBuilder<KasserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new KasserDbContext(options);
            _controller = new OrdersController(_context);

            // Setup user claims for multi-tenant context
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

            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            var store = new Store { Id = 1, Name = "Test Store", IsActive = true };
            _context.Stores.Add(store);

            var category = new Category { Id = 1, Name = "مشروبات", StoreId = 1 };
            _context.Categories.Add(category);

            var product = new Product
            {
                Id = 1,
                Name = "كولا",
                Price = 15.00m,
                Stock = 100,
                IsAvailable = true,
                CategoryId = 1,
                StoreId = 1
            };
            _context.Products.Add(product);

            var settings = new AppSettings
            {
                Id = 1,
                StoreId = 1,
                TaxEnabled = true,
                TaxRate = 14m,
                StoreName = "Test Store",
                Currency = "ج.م"
            };
            _context.AppSettings.Add(settings);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetOrders_ReturnsEmptyList_WhenNoOrders()
        {
            // Act
            var result = await _controller.GetOrders();

            // Assert - GetOrders returns list directly in Value property
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateOrder_WithValidData_ReturnsCreatedOrder()
        {
            // Arrange
            var orderDto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = 1,
                        Quantity = 2,
                        PriceAtTime = 15.00m
                    }
                },
                PaymentMethod = "Cash",
                Discount = 0
            };

            // Act
            var result = await _controller.CreateOrder(orderDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            var order = createdResult?.Value as OrderDto;
            order.Should().NotBeNull();
            order!.Items.Should().HaveCount(1);
            order.Subtotal.Should().Be(30.00m); // 2 * 15
        }

        [Fact]
        public async Task CreateOrder_WithEmptyCart_ReturnsBadRequest()
        {
            // Arrange
            var orderDto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>(),
                PaymentMethod = "Cash"
            };

            // Act
            var result = await _controller.CreateOrder(orderDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateOrder_WithInsufficientStock_ReturnsBadRequest()
        {
            // Arrange
            var orderDto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = 1,
                        Quantity = 999, // More than available stock (100)
                        PriceAtTime = 15.00m
                    }
                },
                PaymentMethod = "Cash"
            };

            // Act
            var result = await _controller.CreateOrder(orderDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateOrder_UpdatesProductStock()
        {
            // Arrange
            var initialStock = 100;
            var orderQuantity = 5;

            var orderDto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = 1,
                        Quantity = orderQuantity,
                        PriceAtTime = 15.00m
                    }
                },
                PaymentMethod = "Cash"
            };

            // Act
            await _controller.CreateOrder(orderDto);

            // Assert
            var product = await _context.Products.FindAsync(1);
            product!.Stock.Should().Be(initialStock - orderQuantity);
        }

        [Fact]
        public async Task GetOrder_WithValidId_ReturnsOrder()
        {
            // Arrange - Create an order first
            var order = new Order
            {
                OrderNumber = "20251213-0001",
                StoreId = 1,
                Subtotal = 30.00m,
                PaymentMethod = "Cash",
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2, PriceAtTime = 15.00m }
                }
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetOrder(order.Id);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.OrderNumber.Should().Be("20251213-0001");
        }

        [Fact]
        public async Task GetOrder_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetOrder(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
