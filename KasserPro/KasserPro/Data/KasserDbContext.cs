using KasserPro.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KasserPro.Api.Data
{
    public class KasserDbContext : DbContext
    {
        public KasserDbContext(DbContextOptions<KasserDbContext> options) : base(options) { }

        // الجداول (DbSets)
        public DbSet<Store> Stores { get; set; }          // جدول المتاجر
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }   // جدول التصنيفات الجديد
        public DbSet<User> Users { get; set; }            // جدول المستخدمين الجديد
        public DbSet<AppSettings> AppSettings { get; set; } // جدول الإعدادات

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ═══════════════════════════════════════════════════════════
            // 1. تكوين Order
            // ═══════════════════════════════════════════════════════════
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderNumber);
            
            // TaxRate و Discount بدقة عالية
            modelBuilder.Entity<Order>()
                .Property(o => o.TaxRate)
                .HasPrecision(5, 4); // مثال: 0.1400
            
            modelBuilder.Entity<Order>()
                .Property(o => o.Discount)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<Order>()
                .Property(o => o.Subtotal)
                .HasPrecision(18, 2);

            // ═══════════════════════════════════════════════════════════
            // 2. تكوين Product
            // ═══════════════════════════════════════════════════════════
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // ═══════════════════════════════════════════════════════════
            // 3. العلاقات (Relationships)
            // ═══════════════════════════════════════════════════════════
            
            // OrderItem → Order
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // لو حذفنا Order تنحذف OrderItems بتاعته
            
            // OrderItem → Product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // مينفعش نحذف Product موجود في Order
            
            // Product → Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); // لو حذفنا Category، CategoryId يبقى null
            
            // Order → User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.SetNull); // لو حذفنا User، UserId يبقى null
            
            // Store Relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Store)
                .WithMany(s => s.Categories)
                .HasForeignKey(c => c.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Store)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.Store)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<AppSettings>()
                .HasOne(a => a.Store)
                .WithMany()
                .HasForeignKey(a => a.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            // ═══════════════════════════════════════════════════════════
            // 4. فهارس لتحسين الأداء (Indexes)
            // ═══════════════════════════════════════════════════════════
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique(); // OrderNumber لازم يكون فريد
            
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CreatedAt); // للبحث بالتاريخ بسرعة
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique(); // Username لازم يكون فريد
            
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name); // للبحث بالاسم بسرعة

            // ═══════════════════════════════════════════════════════════
            // 5. بيانات تجريبية (Seed Data)
            // ═══════════════════════════════════════════════════════════
            
            // إضافة متجر افتراضي
            modelBuilder.Entity<Store>().HasData(
                new Store 
                { 
                    Id = 1, 
                    Name = "متجر تجريبي", 
                    Phone = "01000000000",
                    Address = "القاهرة، مصر",
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1)
                }
            );
            
            // إضافة تصنيفات افتراضية (مرتبطة بالمتجر)
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "مشروبات", Color = "#3B82F6", Icon = "🥤", StoreId = 1 },
                new Category { Id = 2, Name = "وجبات", Color = "#EF4444", Icon = "🍔", StoreId = 1 },
                new Category { Id = 3, Name = "حلويات", Color = "#F59E0B", Icon = "🍰", StoreId = 1 },
                new Category { Id = 4, Name = "مقبلات", Color = "#10B981", Icon = "🍟", StoreId = 1 }
            );

            // إضافة مستخدم افتراضي (Owner للمتجر)
            // كلمة المرور: admin123 (مشفرة بـ BCrypt)
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "admin", 
                    PasswordHash = "$2a$11$5EqkFvM3Y8LZZ0YHqVn4ZeOxNc7DWYpCvLZU3p6qWGNJxJ8HK9EzS", // admin123
                    FullName = "المدير العام",
                    Role = "Owner",
                    IsActive = true,
                    StoreId = 1,
                    CreatedAt = new DateTime(2025, 1, 1)
                }
            );

            // إضافة أصناف افتراضية (مرتبطة بالمتجر)
            modelBuilder.Entity<Product>().HasData(
                // مشروبات
                new Product { Id = 1, Name = "كولا", Price = 15.00m, CategoryId = 1, Stock = 100, IsAvailable = true, StoreId = 1 },
                new Product { Id = 2, Name = "عصير برتقال", Price = 20.00m, CategoryId = 1, Stock = 50, IsAvailable = true, StoreId = 1 },
                new Product { Id = 3, Name = "مياه معدنية", Price = 10.00m, CategoryId = 1, Stock = 200, IsAvailable = true, StoreId = 1 },
                
                // وجبات
                new Product { Id = 4, Name = "برجر لحم", Price = 50.00m, CategoryId = 2, Stock = 30, IsAvailable = true, StoreId = 1 },
                new Product { Id = 5, Name = "برجر دجاج", Price = 45.00m, CategoryId = 2, Stock = 35, IsAvailable = true, StoreId = 1 },
                new Product { Id = 6, Name = "بيتزا مارجريتا", Price = 80.00m, CategoryId = 2, Stock = 20, IsAvailable = true, StoreId = 1 },
                
                // حلويات
                new Product { Id = 7, Name = "كيكة شوكولاتة", Price = 25.00m, CategoryId = 3, Stock = 15, IsAvailable = true, StoreId = 1 },
                new Product { Id = 8, Name = "آيس كريم", Price = 30.00m, CategoryId = 3, Stock = 40, IsAvailable = true, StoreId = 1 },
                
                // مقبلات
                new Product { Id = 9, Name = "بطاطس", Price = 20.00m, CategoryId = 4, Stock = 60, IsAvailable = true, StoreId = 1 },
                new Product { Id = 10, Name = "أصابع موتزاريلا", Price = 35.00m, CategoryId = 4, Stock = 25, IsAvailable = true, StoreId = 1 }
            );

            // إعدادات التطبيق الافتراضية
            modelBuilder.Entity<AppSettings>().HasData(
                new AppSettings
                {
                    Id = 1,
                    TaxEnabled = true,
                    TaxRate = 14m,
                    StoreName = "KasserPro",
                    Currency = "ج.م",
                    StoreId = 1
                }
            );
        }
    }
}