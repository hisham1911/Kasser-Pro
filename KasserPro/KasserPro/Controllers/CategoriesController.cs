using KasserPro.Api.Data;
using KasserPro.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KasserPro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        private readonly KasserDbContext _context;

        public CategoriesController(KasserDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        // جلب كل التصنيفات
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCategories()
        {
            var storeId = GetStoreId();
            
            var categories = await _context.Categories
                .Where(c => c.StoreId == storeId)
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Color,
                    c.Icon,
                    ProductsCount = _context.Products.Count(p => p.CategoryId == c.Id && p.StoreId == storeId)
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/categories/5
        // جلب تصنيف واحد بالـ Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var storeId = GetStoreId();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (category == null)
            {
                return NotFound(new { message = "التصنيف غير موجود" });
            }

            return category;
        }

        // GET: api/categories/5/products
        // جلب كل الأصناف في تصنيف معين
        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCategoryProducts(int id)
        {
            var storeId = GetStoreId();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (category == null)
            {
                return NotFound(new { message = "التصنيف غير موجود" });
            }

            var products = await _context.Products
                .Where(p => p.CategoryId == id && p.StoreId == storeId)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return products;
        }

        // POST: api/categories
        // إضافة تصنيف جديد
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var storeId = GetStoreId();
            category.StoreId = storeId;
            
            // التحقق من عدم وجود تصنيف بنفس الاسم في نفس المتجر
            var exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower() && c.StoreId == storeId);

            if (exists)
            {
                return BadRequest(new { message = "يوجد تصنيف بنفس الاسم بالفعل" });
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // PUT: api/categories/5
        // تعديل تصنيف موجود
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            var storeId = GetStoreId();
            
            if (id != category.Id)
            {
                return BadRequest(new { message = "رقم التصنيف غير متطابق" });
            }

            // التحقق من وجود التصنيف في نفس المتجر
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);
            if (existingCategory == null)
            {
                return NotFound(new { message = "التصنيف غير موجود" });
            }

            // التحقق من عدم تكرار الاسم في نفس المتجر
            var duplicate = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower() && c.Id != id && c.StoreId == storeId);

            if (duplicate)
            {
                return BadRequest(new { message = "يوجد تصنيف آخر بنفس الاسم" });
            }

            // تحديث الحقول
            existingCategory.Name = category.Name;
            existingCategory.Color = category.Color;
            existingCategory.Icon = category.Icon;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء التحديث" });
            }

            return NoContent();
        }

        // DELETE: api/categories/5
        // حذف تصنيف
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var storeId = GetStoreId();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (category == null)
            {
                return NotFound(new { message = "التصنيف غير موجود" });
            }

            // التحقق من عدم وجود أصناف في هذا التصنيف
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id && p.StoreId == storeId);

            if (hasProducts)
            {
                return BadRequest(new { message = "لا يمكن حذف التصنيف لأنه يحتوي على أصناف" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
