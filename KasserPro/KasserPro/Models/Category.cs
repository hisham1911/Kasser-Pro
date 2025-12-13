using System.Text.Json.Serialization;

namespace KasserPro.Api.Models
{
    public class Category
    {
        public int Id { get; set; }                      // رقم التصنيف
        public string Name { get; set; } = string.Empty; // اسم التصنيف (مشروبات، أكل، حلويات)
        public string? Color { get; set; }               // لون التصنيف في الواجهة (اختياري)
        public string? Icon { get; set; }                // أيقونة التصنيف (اختياري)
        
        // ربط بالمتجر
        public int StoreId { get; set; }
        [JsonIgnore]
        public Store? Store { get; set; }
        
        // علاقة: تصنيف واحد فيه أصناف كتير
        [JsonIgnore]
        public List<Product> Products { get; set; } = new();
    }
}
