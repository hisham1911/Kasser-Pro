# 🚀 دليل النشر - KasserPro

## 1️⃣ نشر الـ Backend على Railway

### الخطوة 1: رفع الكود على GitHub
```bash
cd "g:\kasser v1\Kasser-Pro"
git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/YOUR_USERNAME/KasserPro.git
git push -u origin main
```

### الخطوة 2: إنشاء مشروع Railway
1. اذهب إلى [railway.app](https://railway.app)
2. سجل الدخول بـ GitHub
3. اضغط **New Project** → **Deploy from GitHub repo**
4. اختر repository الـ KasserPro
5. اختر مجلد `KasserPro/KasserPro` كـ root

### الخطوة 3: إضافة PostgreSQL
1. في Railway، اضغط **New** → **Database** → **PostgreSQL**
2. Railway سيربط قاعدة البيانات تلقائياً

### الخطوة 4: إعداد Environment Variables
```
DATABASE_URL=<يتم تعيينها تلقائياً>
Jwt__Key=YOUR_SUPER_SECRET_JWT_KEY_MINIMUM_32_CHARACTERS
Jwt__Issuer=KasserPro
Jwt__Audience=KasserProUsers
```

### الخطوة 5: تحديث Connection String
أضف في Railway Environment:
```
ConnectionStrings__DefaultConnection=${{Postgres.DATABASE_URL}}
```

---

## 2️⃣ نشر الـ Frontend على Vercel

### الخطوة 1: تثبيت Vercel CLI
```powershell
npm install -g vercel
```

### الخطوة 2: النشر
```powershell
cd "g:\kasser v1\Kasser-Pro\kasserpro-frontend"
vercel
```
- اختر **Y** لـ setup
- اختر scope الخاص بك
- اضغط Enter للـ defaults

### الخطوة 3: إعداد Environment Variable
في Vercel Dashboard:
1. اذهب للـ Project Settings → Environment Variables
2. أضف:
   - **Name**: `VITE_API_URL`
   - **Value**: `https://YOUR-RAILWAY-APP.railway.app/api`

### الخطوة 4: إعادة النشر
```powershell
vercel --prod
```

---

## ✅ التحقق من النشر

1. **Backend**: `https://YOUR-APP.railway.app/api/health`
2. **Frontend**: `https://YOUR-APP.vercel.app`
3. **Swagger**: `https://YOUR-APP.railway.app/swagger`

---

## 🔧 ملاحظات مهمة

- تأكد من تغيير `Jwt:Key` لقيمة سرية قوية
- Railway يعطيك 500 ساعة مجانية شهرياً
- Vercel مجاني للمشاريع الشخصية
