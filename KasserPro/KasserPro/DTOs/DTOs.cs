using System.ComponentModel.DataAnnotations;

namespace KasserPro.Api.DTOs
{
    // ============ Category DTOs ============
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public int ProductsCount { get; set; }
    }

    // ============ Product DTOs ============
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryColor { get; set; }
        public string? CategoryIcon { get; set; }
    }

    // ============ Order DTOs ============
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
        public decimal Total => PriceAtTime * Quantity;
    }

    // ============ Create DTOs ============
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "يجب إضافة منتجات للطلب")]
        [MinLength(1, ErrorMessage = "يجب إضافة منتج واحد على الأقل")]
        public List<CreateOrderItemDto> Items { get; set; } = new();
        
        [Range(0, double.MaxValue, ErrorMessage = "الخصم يجب أن يكون قيمة موجبة")]
        public decimal Discount { get; set; } = 0;
        
        [Required]
        [RegularExpression("^(Cash|Card|Wallet)$", ErrorMessage = "طريقة الدفع غير صالحة")]
        public string PaymentMethod { get; set; } = "Cash";
    }

    public class CreateOrderItemDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "رقم المنتج غير صالح")]
        public int ProductId { get; set; }
        
        [Range(1, 9999, ErrorMessage = "الكمية يجب أن تكون بين 1 و 9999")]
        public int Quantity { get; set; } = 1;
        
        [Range(0.01, 999999, ErrorMessage = "السعر غير صالح")]
        public decimal PriceAtTime { get; set; }
    }

    public class CreateProductDto
    {
        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "اسم المنتج يجب أن يكون بين 2 و 100 حرف")]
        public string Name { get; set; } = string.Empty;
        
        [Range(0.01, 999999, ErrorMessage = "السعر يجب أن يكون أكبر من 0")]
        public decimal Price { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "المخزون يجب أن يكون قيمة موجبة")]
        public int Stock { get; set; } = 0;
        
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم التصنيف يجب أن يكون بين 2 و 50 حرف")]
        public string Name { get; set; } = string.Empty;
        
        public string? Color { get; set; }
        public string? Icon { get; set; }
    }

    // ============ Auth DTOs ============
    public class LoginDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "اسم المستخدم يجب أن يكون بين 3 و 50 حرف")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "اسم المتجر مطلوب")]
        [StringLength(100)]
        public string StoreName { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "رقم الهاتف غير صالح")]
        public string? Phone { get; set; }
    }

    public class AddUserDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [RegularExpression("^(Cashier|Manager)$", ErrorMessage = "الدور يجب أن يكون Cashier أو Manager")]
        public string Role { get; set; } = "Cashier";
    }

    // ============ Settings DTOs ============
    public class TaxSettingsDto
    {
        public bool TaxEnabled { get; set; }
        
        [Range(0, 100, ErrorMessage = "نسبة الضريبة يجب أن تكون بين 0 و 100")]
        public decimal TaxRate { get; set; }
    }

    // ============ Pagination ============
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}

