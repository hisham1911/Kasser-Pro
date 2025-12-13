using System.Text.Json.Serialization;

namespace KasserPro.Api.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;           // اسم المتجر
        public string? Phone { get; set; }                         // رقم الهاتف
        public string? Address { get; set; }                       // العنوان
        public string? Logo { get; set; }                          // شعار المتجر (URL)
        public bool IsActive { get; set; } = true;                 // هل المتجر نشط؟
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;    // تاريخ الإنشاء
        public DateTime? ExpiresAt { get; set; }                   // تاريخ انتهاء الاشتراك (اختياري)
        
        // العلاقات
        [JsonIgnore]
        public List<User> Users { get; set; } = new();
        [JsonIgnore]
        public List<Product> Products { get; set; } = new();
        [JsonIgnore]
        public List<Category> Categories { get; set; } = new();
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new();
    }
}
