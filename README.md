<p align="center">
  <img src="https://img.icons8.com/3d-fluency/94/cash-register.png" alt="KasserPro Logo" width="120"/>
</p>

<h1 align="center">🧾 KasserPro - نظام نقاط البيع</h1>

<p align="center">
  <strong>نظام كاشير متكامل وحديث للمحلات والمطاعم والكافيهات</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8"/>
  <img src="https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge&logo=react&logoColor=black" alt="React 19"/>
  <img src="https://img.shields.io/badge/Vite-7-646CFF?style=for-the-badge&logo=vite&logoColor=white" alt="Vite"/>
  <img src="https://img.shields.io/badge/Tailwind-3.4-06B6D4?style=for-the-badge&logo=tailwindcss&logoColor=white" alt="Tailwind CSS"/>
  <img src="https://img.shields.io/badge/PostgreSQL-Database-336791?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL"/>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/License-MIT-green?style=flat-square" alt="License"/>
  <img src="https://img.shields.io/badge/Status-Active-success?style=flat-square" alt="Status"/>
  <img src="https://img.shields.io/badge/PRs-Welcome-brightgreen?style=flat-square" alt="PRs Welcome"/>
</p>

---

## 📋 نظرة عامة

**KasserPro** هو نظام نقاط بيع (POS) عصري ومتكامل، مصمم خصيصاً للسوق العربي. يتميز بواجهة مستخدم أنيقة وسهلة الاستخدام، مع دعم كامل للغة العربية واتجاه RTL.

### ✨ لماذا KasserPro؟

| الميزة | الوصف |
|--------|-------|
| 🎨 **واجهة عصرية** | تصميم Dark Mode أنيق ومريح للعين |
| 🌍 **دعم العربية** | واجهة عربية كاملة مع اتجاه RTL |
| ⚡ **سريع وخفيف** | بناء على أحدث التقنيات لأداء فائق |
| 📱 **متجاوب** | يعمل على جميع الشاشات والأجهزة |
| 🔒 **آمن** | JWT Authentication مع تشفير BCrypt |
| 🏪 **Multi-tenant** | دعم متاجر متعددة بعزل كامل للبيانات |
| 🛠️ **قابل للتخصيص** | إعدادات مرنة تناسب احتياجاتك |

---

## 🚀 المميزات المُنجزة ✅

### 🔐 نظام المصادقة والصلاحيات
- ✅ تسجيل الدخول بـ JWT Token (صلاحية 7 أيام)
- ✅ تسجيل متجر جديد مع حساب Owner
- ✅ إضافة موظفين للمتجر
- ✅ صلاحيات متعددة (Owner, Manager, Cashier, SuperAdmin)
- ✅ حماية الـ Routes حسب الصلاحية
- ✅ تشفير كلمات المرور بـ BCrypt

### 🏪 نظام Multi-Tenant
- ✅ كل متجر له بياناته المعزولة
- ✅ فلترة تلقائية بـ StoreId
- ✅ إعدادات مستقلة لكل متجر

### 💳 نقطة البيع (POS)
- ✅ واجهة بيع سريعة وسهلة الاستخدام
- ✅ عرض المنتجات بشكل مرئي جذاب مع الصور
- ✅ فلترة المنتجات حسب التصنيف
- ✅ بحث سريع في المنتجات
- ✅ سلة مشتريات ذكية مع التحقق من المخزون
- ✅ حساب تلقائي للضريبة (قابل للتفعيل/التعطيل)
- ✅ دعم الخصومات
- ✅ طرق دفع متعددة (كاش - بطاقة - محفظة)
- ✅ طباعة الفواتير الحرارية (ESC/POS)

### 📦 إدارة المنتجات
- ✅ إضافة وتعديل وحذف المنتجات
- ✅ تصنيف المنتجات في فئات
- ✅ إدارة المخزون والكميات
- ✅ تحديث المخزون تلقائياً عند البيع
- ✅ تحديد حالة توفر المنتج
- ✅ منع تفعيل منتج بمخزون صفر
- ✅ إضافة صور للمنتجات

### 🏷️ إدارة التصنيفات
- ✅ تنظيم المنتجات في فئات
- ✅ تخصيص ألوان وأيقونات للتصنيفات
- ✅ سهولة التنقل بين الفئات

### 📋 إدارة الطلبات
- ✅ عرض سجل الطلبات مع Pagination
- ✅ تفاصيل كل طلب مع الأصناف
- ✅ أرقام طلبات فريدة (YYYYMMDD-XXXX)
- ✅ حفظ سعر المنتج وقت البيع
- ✅ طباعة إيصال الطلب

### ⚙️ الإعدادات
- ✅ تفعيل/تعطيل الضريبة
- ✅ تحديد نسبة الضريبة
- ✅ تخصيص اسم المتجر
- ✅ تحديد العملة

---

## 🔲 الميزات الناقصة (خارطة الطريق)

### 📊 التقارير والإحصائيات (أولوية عالية)
- 🔲 لوحة تحكم Dashboard بالرسوم البيانية
- 🔲 تقرير المبيعات اليومية/الأسبوعية/الشهرية
- 🔲 تقرير الأرباح والخسائر
- 🔲 تقرير المنتجات الأكثر مبيعاً
- 🔲 تقرير حركة المخزون
- 🔲 تصدير التقارير (PDF/Excel)

### � إaدارة المخزون المتقدمة
- 🔲 تنبيهات نفاد المخزون
- 🔲 تقارير جرد المخزون
- 🔲 تاريخ حركة المخزون (دخول/خروج)
- 🔲 دعم الباركود والـ QR Code
- 🔲 ماسح الباركود

### 👥 إدارة العملاء
- 🔲 قاعدة بيانات العملاء
- 🔲 نظام نقاط الولاء
- 🔲 تاريخ مشتريات العميل
- 🔲 الفواتير الآجلة (الدين)

### 🏭 إدارة الموردين والمشتريات
- 🔲 قاعدة بيانات الموردين
- 🔲 فواتير الشراء
- 🔲 تتبع المدفوعات للموردين

### 💰 ميزات مالية
- 🔲 إدارة الصندوق (فتح/إغلاق الوردية)
- � تقريتر الكاش في الدرج
- 🔲 المصروفات اليومية
- 🔲 تقارير الضرائب
- 🔲 الفاتورة الإلكترونية

### 🔧 ميزات إضافية
- 🔲 العمل بدون إنترنت (Offline mode)
- 🔲 النسخ الاحتياطي للبيانات
- 🔲 تطبيق موبايل (iOS/Android)
- 🔲 نظام الكوبونات والعروض
- 🔲 إشعارات Push

---

## 🏗️ البنية التقنية

```
KasserPro/
├── 📁 KasserPro/                    # Backend - ASP.NET Core 8
│   ├── 📁 Controllers/              # API Controllers
│   │   ├── AuthController.cs        # المصادقة وتسجيل الدخول
│   │   ├── BaseApiController.cs     # Controller أساسي مع StoreId
│   │   ├── CategoriesController.cs  # إدارة التصنيفات
│   │   ├── OrdersController.cs      # إدارة الطلبات
│   │   ├── ProductsController.cs    # إدارة المنتجات
│   │   └── SettingsController.cs    # الإعدادات
│   ├── 📁 Models/                   # Data Models
│   │   ├── Store.cs                 # المتجر
│   │   ├── User.cs                  # المستخدم
│   │   ├── Product.cs               # المنتج
│   │   ├── Category.cs              # التصنيف
│   │   ├── Order.cs                 # الطلب
│   │   ├── OrderItem.cs             # عنصر الطلب
│   │   └── AppSettings.cs           # الإعدادات
│   ├── 📁 DTOs/                     # Data Transfer Objects
│   ├── 📁 Data/                     # Database Context
│   │   └── KasserDbContext.cs       # EF Core Context
│   ├── 📁 Migrations/               # EF Core Migrations
│   └── Program.cs                   # Entry Point
│
└── 📁 kasserpro-frontend/           # Frontend - React 19
    ├── 📁 src/
    │   ├── 📁 components/           # Reusable Components
    │   │   ├── Header.jsx
    │   │   └── ProtectedRoute.jsx
    │   ├── 📁 context/              # React Context
    │   │   └── AuthContext.jsx      # حالة المصادقة
    │   ├── 📁 hooks/                # Custom Hooks
    │   │   ├── useProducts.js
    │   │   ├── useCategories.js
    │   │   └── useOrders.js
    │   ├── 📁 pages/                # Page Components
    │   │   ├── Login.jsx            # تسجيل الدخول
    │   │   ├── Register.jsx         # تسجيل متجر جديد
    │   │   ├── POS.jsx              # نقطة البيع
    │   │   ├── Products.jsx         # إدارة المنتجات
    │   │   ├── Orders.jsx           # سجل الطلبات
    │   │   └── Settings.jsx         # الإعدادات
    │   ├── 📁 services/             # API Services
    │   │   └── api.js
    │   ├── App.jsx
    │   └── main.jsx
    ├── tailwind.config.js
    ├── vite.config.js
    └── package.json
```

---

## 🛠️ التقنيات المستخدمة

### Backend
| التقنية | الإصدار | الوصف |
|---------|---------|-------|
| ![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet) | 8.0 | إطار العمل الأساسي |
| ![EF Core](https://img.shields.io/badge/EF_Core-8.0-512BD4?logo=dotnet) | 8.0 | ORM للتعامل مع قاعدة البيانات |
| ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql) | 16 | قاعدة البيانات (Production) |
| ![SQLite](https://img.shields.io/badge/SQLite-3-003B57?logo=sqlite) | 3 | قاعدة البيانات (Development) |
| ![JWT](https://img.shields.io/badge/JWT-Auth-000000?logo=jsonwebtokens) | - | المصادقة |
| ![BCrypt](https://img.shields.io/badge/BCrypt-Hash-green) | - | تشفير كلمات المرور |
| ![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger) | 3.0 | توثيق API |

### Frontend
| التقنية | الإصدار | الوصف |
|---------|---------|-------|
| ![React](https://img.shields.io/badge/React-19-61DAFB?logo=react) | 19.2 | مكتبة بناء الواجهات |
| ![Vite](https://img.shields.io/badge/Vite-7-646CFF?logo=vite) | 7.2 | أداة البناء السريعة |
| ![Tailwind](https://img.shields.io/badge/Tailwind-3.4-06B6D4?logo=tailwindcss) | 3.4 | إطار CSS |
| ![React Query](https://img.shields.io/badge/React_Query-5-FF4154?logo=reactquery) | 5.90 | إدارة حالة السيرفر |
| ![React Router](https://img.shields.io/badge/React_Router-7-CA4245?logo=reactrouter) | 7.10 | التنقل بين الصفحات |
| ![Axios](https://img.shields.io/badge/Axios-1.13-5A29E4?logo=axios) | 1.13 | HTTP Client |
| ![Heroicons](https://img.shields.io/badge/Heroicons-2-8B5CF6) | 2.2 | أيقونات |

---

## ⚡ التثبيت والتشغيل

### المتطلبات الأساسية
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/) (للإنتاج) أو SQLite (للتطوير)
- [Git](https://git-scm.com/)

### 1️⃣ استنساخ المشروع
```bash
git clone https://github.com/YOUR_USERNAME/KasserPro.git
cd KasserPro
```

### 2️⃣ تشغيل Backend
```bash
# الانتقال لمجلد الـ Backend
cd KasserPro/KasserPro

# تشغيل الـ Migrations (أول مرة فقط)
dotnet ef database update

# تشغيل السيرفر
dotnet run
```

> 🌐 Backend يعمل على: `http://localhost:5299`
> 
> 📚 Swagger UI: `http://localhost:5299/swagger`
>
> 🏥 Health Check: `http://localhost:5299/api/health`

### 3️⃣ تشغيل Frontend
```bash
# فتح Terminal جديد
cd kasserpro-frontend

# تثبيت المكتبات
npm install

# تشغيل السيرفر
npm run dev
```

> 🌐 Frontend يعمل على: `http://localhost:5173`

### 4️⃣ بيانات الدخول الافتراضية
```
اسم المستخدم: admin
كلمة المرور: admin123
```

---

## 📡 API Endpoints

### Auth - المصادقة
| Method | Endpoint | الوصف |
|--------|----------|-------|
| `POST` | `/api/auth/login` | تسجيل الدخول |
| `POST` | `/api/auth/register` | تسجيل متجر جديد |
| `POST` | `/api/auth/add-user` | إضافة موظف (Owner/Manager) |
| `GET` | `/api/auth/me` | معلومات المستخدم الحالي |

### Products - المنتجات
| Method | Endpoint | الوصف |
|--------|----------|-------|
| `GET` | `/api/products` | جلب جميع المنتجات (مع فلترة) |
| `GET` | `/api/products/{id}` | جلب منتج محدد |
| `POST` | `/api/products` | إضافة منتج جديد |
| `PUT` | `/api/products/{id}` | تعديل منتج |
| `DELETE` | `/api/products/{id}` | حذف منتج |
| `PATCH` | `/api/products/{id}/stock` | تحديث المخزون |
| `PATCH` | `/api/products/{id}/availability` | تغيير حالة التوفر |

### Categories - التصنيفات
| Method | Endpoint | الوصف |
|--------|----------|-------|
| `GET` | `/api/categories` | جلب جميع التصنيفات |
| `GET` | `/api/categories/{id}` | جلب تصنيف محدد |
| `GET` | `/api/categories/{id}/products` | منتجات التصنيف |
| `POST` | `/api/categories` | إضافة تصنيف |
| `PUT` | `/api/categories/{id}` | تعديل تصنيف |
| `DELETE` | `/api/categories/{id}` | حذف تصنيف |

### Orders - الطلبات
| Method | Endpoint | الوصف |
|--------|----------|-------|
| `GET` | `/api/orders` | جلب جميع الطلبات (Paginated) |
| `GET` | `/api/orders/{id}` | جلب طلب محدد |
| `POST` | `/api/orders` | إنشاء طلب جديد |
| `GET` | `/api/orders/{id}/print` | طباعة إيصال الطلب |

### Settings - الإعدادات
| Method | Endpoint | الوصف |
|--------|----------|-------|
| `GET` | `/api/settings` | جلب الإعدادات |
| `PUT` | `/api/settings` | تحديث الإعدادات |
| `PATCH` | `/api/settings/tax` | تحديث إعدادات الضريبة |

---

## 📊 نموذج البيانات

```mermaid
erDiagram
    Store ||--o{ User : has
    Store ||--o{ Product : has
    Store ||--o{ Category : has
    Store ||--o{ Order : has
    Store ||--|| AppSettings : has
    
    Category ||--o{ Product : contains
    Product ||--o{ OrderItem : "ordered in"
    Order ||--|{ OrderItem : contains
    User ||--o{ Order : creates

    Store {
        int Id PK
        string Name
        string Phone
        string Address
        string Logo
        bool IsActive
        datetime CreatedAt
        datetime ExpiresAt
    }

    User {
        int Id PK
        string Username UK
        string PasswordHash
        string FullName
        string Role
        bool IsActive
        int StoreId FK
        datetime CreatedAt
    }

    Category {
        int Id PK
        string Name
        string Color
        string Icon
        int StoreId FK
    }

    Product {
        int Id PK
        string Name
        decimal Price
        int CategoryId FK
        string ImageUrl
        int Stock
        bool IsAvailable
        int StoreId FK
    }

    Order {
        int Id PK
        string OrderNumber UK
        datetime CreatedAt
        decimal Subtotal
        decimal Discount
        decimal TaxRate
        string PaymentMethod
        int UserId FK
        int StoreId FK
        string Notes
    }

    OrderItem {
        int Id PK
        int OrderId FK
        int ProductId FK
        int Quantity
        decimal PriceAtTime
    }

    AppSettings {
        int Id PK
        bool TaxEnabled
        decimal TaxRate
        string StoreName
        string Currency
        int StoreId FK
    }
```

---

## 🚀 النشر (Deployment)

### Railway (موصى به)
المشروع جاهز للنشر على Railway مع:
- PostgreSQL database
- Automatic migrations
- Health check endpoint

راجع ملف `RAILWAY_DEPLOYMENT.md` للتفاصيل.

### متغيرات البيئة
```env
DATABASE_URL=postgresql://user:password@host:port/database
JWT_KEY=your-secret-key-min-32-chars
JWT_ISSUER=KasserPro
JWT_AUDIENCE=KasserProUsers
```

---

## 🤝 المساهمة

نرحب بمساهماتكم! إذا كنت ترغب في المساهمة:

1. **Fork** المشروع
2. أنشئ **Branch** جديد (`git checkout -b feature/amazing-feature`)
3. **Commit** تغييراتك (`git commit -m 'Add amazing feature'`)
4. **Push** للـ Branch (`git push origin feature/amazing-feature`)
5. افتح **Pull Request**

---

## 📄 الرخصة

هذا المشروع مرخص تحت رخصة **MIT** - راجع ملف [LICENSE](LICENSE) للتفاصيل.

---

## 👨‍💻 المطور

<p align="center">
  <strong>تم التطوير بـ ❤️</strong>
</p>

---

<p align="center">
  <strong>⭐ إذا أعجبك المشروع، لا تنسى إضافة نجمة! ⭐</strong>
</p>
