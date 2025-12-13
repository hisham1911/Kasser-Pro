using KasserPro.Api.Data;
using KasserPro.Api.DTOs;
using KasserPro.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KasserPro.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly KasserDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(KasserDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Store)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
            {
                return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
            }

            if (!user.IsActive)
            {
                return Unauthorized(new { message = "الحساب غير مفعل" });
            }

            if (user.Store != null && !user.Store.IsActive)
            {
                return Unauthorized(new { message = "المتجر غير مفعل" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.FullName,
                    user.Role,
                    user.StoreId,
                    StoreName = user.Store?.Name
                }
            });
        }

        // POST: api/auth/register (تسجيل متجر جديد)
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            // التحقق من عدم وجود اسم مستخدم مكرر
            var userExists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
            if (userExists)
            {
                return BadRequest(new { message = "اسم المستخدم موجود بالفعل" });
            }

            // إنشاء متجر جديد
            var store = new Store
            {
                Name = dto.StoreName,
                Phone = dto.Phone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            // إنشاء مستخدم Owner للمتجر
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Role = "Owner",
                IsActive = true,
                StoreId = store.Id,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // إنشاء إعدادات افتراضية للمتجر (اختياري)
            
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "تم إنشاء الحساب بنجاح",
                token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.FullName,
                    user.Role,
                    user.StoreId,
                    StoreName = store.Name
                }
            });
        }

        // POST: api/auth/add-user (إضافة موظف للمتجر - للـ Owner/Manager فقط)
        [HttpPost("add-user")]
        public async Task<ActionResult> AddUser(AddUserDto dto)
        {
            // التحقق من التوكن والصلاحيات
            var currentUser = await GetCurrentUser();
            if (currentUser == null)
            {
                return Unauthorized(new { message = "يجب تسجيل الدخول أولاً" });
            }

            if (currentUser.Role != "Owner" && currentUser.Role != "Manager")
            {
                return Forbid();
            }

            // التحقق من عدم تكرار اسم المستخدم
            var userExists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
            if (userExists)
            {
                return BadRequest(new { message = "اسم المستخدم موجود بالفعل" });
            }

            // التحقق من الدور المسموح
            if (dto.Role == "Owner")
            {
                return BadRequest(new { message = "لا يمكن إنشاء حساب Owner آخر" });
            }

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Role = dto.Role,
                IsActive = true,
                StoreId = currentUser.StoreId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "تم إضافة المستخدم بنجاح",
                user = new
                {
                    user.Id,
                    user.Username,
                    user.FullName,
                    user.Role
                }
            });
        }

        // GET: api/auth/me (معلومات المستخدم الحالي)
        [HttpGet("me")]
        public async Task<ActionResult> GetMe()
        {
            var user = await GetCurrentUser();
            if (user == null)
            {
                return Unauthorized(new { message = "يجب تسجيل الدخول أولاً" });
            }

            return Ok(new
            {
                user.Id,
                user.Username,
                user.FullName,
                user.Role,
                user.StoreId,
                StoreName = user.Store?.Name
            });
        }

        // Helper: توليد JWT Token
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? "KasserProSecretKey123456789012345678901234567890"));
            
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("StoreId", user.StoreId.ToString()),
                new Claim("FullName", user.FullName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "KasserPro",
                audience: _configuration["Jwt:Audience"] ?? "KasserProUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Helper: الحصول على المستخدم الحالي من التوكن
        private async Task<User?> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return null;

            var userId = int.Parse(userIdClaim.Value);
            return await _context.Users
                .Include(u => u.Store)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }

}
