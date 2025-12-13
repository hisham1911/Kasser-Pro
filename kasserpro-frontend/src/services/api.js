import axios from "axios";

const API_URL = "http://localhost:5108/api";

// Helper functions for safe localStorage access
const getStorageItem = (key) => {
  try {
    return localStorage.getItem(key);
  } catch (error) {
    console.warn("localStorage not available:", error);
    return null;
  }
};

const removeStorageItem = (key) => {
  try {
    localStorage.removeItem(key);
  } catch (error) {
    console.warn("Failed to remove from localStorage:", error);
  }
};

// إعداد Axios
const api = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor لإضافة Token لكل الطلبات
api.interceptors.request.use(
  (config) => {
    const token = getStorageItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor للتعامل مع أخطاء 401
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Log error to console for debugging
    console.error("❌ API Error:", {
      url: error.config?.url,
      method: error.config?.method,
      status: error.response?.status,
      data: error.response?.data,
      message: error.message,
    });

    if (error.response?.status === 401) {
      // Token expired or invalid
      removeStorageItem("token");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

// ============ Auth API ============
export const authApi = {
  // تسجيل الدخول
  login: (username, password) =>
    api.post("/auth/login", { username, password }),

  // تسجيل متجر جديد
  register: (data) => api.post("/auth/register", data),

  // إضافة مستخدم للمتجر
  addUser: (data) => api.post("/auth/add-user", data),

  // معلومات المستخدم الحالي
  me: () => api.get("/auth/me"),
};

// ============ Categories API ============
export const categoriesApi = {
  // جلب كل التصنيفات
  getAll: () => api.get("/categories"),

  // جلب تصنيف واحد
  getById: (id) => api.get(`/categories/${id}`),

  // إضافة تصنيف جديد
  create: (category) => api.post("/categories", category),

  // تعديل تصنيف
  update: (id, category) => api.put(`/categories/${id}`, category),

  // حذف تصنيف
  delete: (id) => api.delete(`/categories/${id}`),
};

// ============ Products API ============
export const productsApi = {
  // جلب كل المنتجات مع الفلاتر
  getAll: (params = {}) => {
    const query = new URLSearchParams();
    if (params.categoryId) query.append("categoryId", params.categoryId);
    if (params.isAvailable !== undefined)
      query.append("isAvailable", params.isAvailable);
    if (params.search) query.append("search", params.search);
    return api.get(`/products?${query.toString()}`);
  },

  // جلب منتج واحد
  getById: (id) => api.get(`/products/${id}`),

  // إضافة منتج جديد
  create: (product) => api.post("/products", product),

  // تعديل منتج
  update: (id, product) => api.put(`/products/${id}`, product),

  // حذف منتج
  delete: (id) => api.delete(`/products/${id}`),

  // تحديث المخزون
  updateStock: (id, stock) => api.patch(`/products/${id}/stock`, stock),

  // تغيير حالة التوفر
  updateAvailability: (id, isAvailable) =>
    api.patch(`/products/${id}/availability`, isAvailable),
};

// ============ Orders API ============
export const ordersApi = {
  // جلب كل الطلبات مع Pagination
  getAll: (page = 1, pageSize = 20) =>
    api.get(`/orders?page=${page}&pageSize=${pageSize}`),

  // جلب طلب واحد
  getById: (id) => api.get(`/orders/${id}`),

  // إنشاء طلب جديد
  create: (order) => api.post("/orders", order),

  // طباعة فاتورة
  print: (id) => api.get(`/orders/${id}/print`, { responseType: "blob" }),
};

// ============ Settings API ============
export const settingsApi = {
  // جلب الإعدادات
  get: () => api.get("/settings"),

  // تحديث الإعدادات
  update: (settings) => api.put("/settings", settings),

  // تحديث إعدادات الضريبة فقط
  updateTax: (taxSettings) => api.patch("/settings/tax", taxSettings),
};

export default api;
