using KasserPro.Api.Data;
using KasserPro.Api.Models;
using KasserPro.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KasserPro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly KasserDbContext _context;

        public OrdersController(KasserDbContext context)
        {
            _context = context;
        }

        // جلب كل الطلبات مع Pagination
        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderDto>>> GetOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var storeId = GetStoreId();
            
            var query = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.StoreId == storeId)
                .OrderByDescending(o => o.CreatedAt);

            var totalCount = await query.CountAsync();
            
            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<OrderDto>
            {
                Items = orders.Select(o => MapToDto(o)).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // إنشاء طلب جديد
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto dto)
        {
            try
            {
                // التحقق من صحة البيانات
                if (dto == null)
                {
                    return BadRequest(new { message = "البيانات المرسلة غير صحيحة" });
                }
                
                if (dto.Items == null || dto.Items.Count == 0)
                {
                    return BadRequest(new { message = "السلة فارغة" });
                }

                var storeId = GetStoreId();
            
                // التحقق من توفر المخزون أولاً
                foreach (var item in dto.Items)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId && p.StoreId == storeId);
                    if (product == null)
                    {
                        return BadRequest(new { message = $"المنتج رقم {item.ProductId} غير موجود" });
                    }
                    if (!product.IsAvailable)
                    {
                        return BadRequest(new { message = $"المنتج {product.Name} غير متاح حالياً" });
                    }
                    if (product.Stock < item.Quantity)
                    {
                        return BadRequest(new { message = $"الكمية المطلوبة من {product.Name} غير متوفرة. المتاح: {product.Stock}" });
                    }
                }

                // توليد رقم الطلب تلقائي (20251208-0001)
                // يجب البحث في كل الطلبات لضمان عدم تكرار الرقم لأن OrderNumber فريد عالمياً
                var today = DateTime.UtcNow.ToString("yyyyMMdd");
                var lastOrderToday = await _context.Orders
                    .Where(o => o.OrderNumber.StartsWith(today))
                    .OrderByDescending(o => o.Id)
                    .FirstOrDefaultAsync();

                int nextNumber = lastOrderToday == null ? 1 :
                    int.Parse(lastOrderToday.OrderNumber.Split('-').Last()) + 1;

                string orderNumber = $"{today}-{nextNumber:D4}";

                // جلب إعدادات الضريبة للمتجر
                var settings = await _context.AppSettings.FirstOrDefaultAsync(s => s.StoreId == storeId);
                var taxRate = (settings?.TaxEnabled == true) ? (settings?.TaxRate ?? 14m) / 100m : 0m;

                var order = new Order
                {
                    OrderNumber = orderNumber,
                    CreatedAt = DateTime.UtcNow,
                    Subtotal = dto.Items.Sum(i => i.PriceAtTime * i.Quantity),
                    Discount = dto.Discount,
                    TaxRate = taxRate,
                    PaymentMethod = dto.PaymentMethod,
                    StoreId = storeId,
                    Items = dto.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        PriceAtTime = i.PriceAtTime
                    }).ToList()
                };

                _context.Orders.Add(order);

                // تحديث المخزون
                foreach (var item in dto.Items)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId && p.StoreId == storeId);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                        // إذا المخزون صفر، نخلي المنتج غير متاح
                        if (product.Stock <= 0)
                        {
                            product.Stock = 0;
                            product.IsAvailable = false;
                        }
                        _context.Products.Update(product);
                    }
                }

                await _context.SaveChangesAsync();

                // جلب الطلب كامل بالأصناف عشان نرجعه
                var fullOrder = await _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, MapToDto(fullOrder!));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the full exception for debugging
                Console.WriteLine($"Error creating order: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { 
                    message = "حدث خطأ أثناء إنشاء الطلب", 
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    details = ex.StackTrace
                });
            }
        }

        // جلب طلب واحد
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var storeId = GetStoreId();
            
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.StoreId == storeId);

            if (order == null) return NotFound();
            return MapToDto(order);
        }
        [HttpGet("{id}/print")]
        public async Task<IActionResult> PrintOrder(int id)
        {
            var storeId = GetStoreId();
            
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.StoreId == storeId);

            if (order == null) return NotFound();
            
            // جلب اسم المتجر من الإعدادات
            var settings = await _context.AppSettings.FirstOrDefaultAsync(s => s.StoreId == storeId);
            var storeName = settings?.StoreName ?? "KasserPro";
            var currency = settings?.Currency ?? "ج.م";

            var receipt = $"       {storeName}\n";
            receipt += "========================\n";
            receipt += $"فاتورة رقم: {order.OrderNumber}\n";
            receipt += $"التاريخ: {order.CreatedAt:yyyy-MM-dd HH:mm}\n";
            receipt += "========================\n";

            foreach (var item in order.Items)
            {
                var productName = item.Product?.Name ?? "منتج";
                receipt += $"{productName} x{item.Quantity}     {item.PriceAtTime * item.Quantity} {currency}\n";
            }

            receipt += "------------------------\n";
            receipt += $"الإجمالي: {order.Subtotal} {currency}\n";
            receipt += $"الخصم: {order.Discount} {currency}\n";
            if (order.TaxAmount > 0)
            {
                receipt += $"الضريبة: {order.TaxAmount} {currency}\n";
            }
            receipt += $"الصافي: {order.Total} {currency}\n";
            receipt += $"طريقة الدفع: {order.PaymentMethod}\n";
            receipt += "========================\n";
            receipt += "     شكرا لزيارتك ❤️\n\n\n";

            // أوامر ESC/POS للطابعة الحرارية
            var escPos = "\x1B\x40" + receipt + "\x1D\x56\x00";  // Initialize + Cut

            return File(System.Text.Encoding.UTF8.GetBytes(escPos), "application/octet-stream");
        }

        // تحويل Order إلى OrderDto
        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.CreatedAt,
                Subtotal = order.Subtotal,
                Discount = order.Discount,
                TaxAmount = order.TaxAmount,
                Total = order.Total,
                PaymentMethod = order.PaymentMethod,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "منتج محذوف",
                    Quantity = i.Quantity,
                    PriceAtTime = i.PriceAtTime
                }).ToList()
            };
        }
    }
}
