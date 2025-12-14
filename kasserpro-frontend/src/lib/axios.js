import axios from "axios";

// Use environment variable for production, fallback to localhost for development
const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5108/api";

const api = axios.create({
    baseURL: API_URL,
    headers: {
        "Content-Type": "application/json",
    },
});

api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("token");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response) {
            // Map status codes to friendly Arabic messages
            switch (error.response.status) {
                case 400:
                    error.message = error.response.data?.message || "بيانات غير صالحة";
                    break;
                case 401:
                    error.message = "انتهت الجلسة، يرجى تسجيل الدخول مرة أخرى";
                    localStorage.removeItem("token");
                    if (!window.location.pathname.includes("/login")) {
                        window.location.href = "/login";
                    }
                    break;
                case 403:
                    error.message = "غير مصرح لك بالقيام بهذا الإجراء";
                    break;
                case 404:
                    error.message = "المورد غير موجود";
                    break;
                case 500:
                    error.message = "حدث خطأ في الخادم، يرجى المحاولة لاحقاً";
                    break;
                default:
                    error.message = "حدث خطأ غير متوقع";
            }
        } else if (error.request) {
            // Network error
            error.message = "فشل الاتصال بالخادم، يرجى التحقق من الإنترنت";
        } else {
            error.message = "حدث خطأ غير متوقع";
        }
        return Promise.reject(error);
    }
);

export default api;
