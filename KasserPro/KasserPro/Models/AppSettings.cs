using System.Text.Json.Serialization;

namespace KasserPro.Api.Models
{
    public class AppSettings
    {
        public int Id { get; set; }
        public bool TaxEnabled { get; set; } = true;
        public decimal TaxRate { get; set; } = 14m; // نسبة مئوية
        public string StoreName { get; set; } = "KasserPro";
        public string Currency { get; set; } = "ج.م";
        
        // Multi-tenant
        public int StoreId { get; set; }
        
        [JsonIgnore]
        public Store? Store { get; set; }
    }
}
