import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { authApi } from '../services/api';
import toast from 'react-hot-toast';

export default function Register() {
  const [formData, setFormData] = useState({
    storeName: '',
    fullName: '',
    phone: '',
    username: '',
    password: '',
    confirmPassword: ''
  });
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Validation
    if (!formData.storeName || !formData.fullName || !formData.username || !formData.password) {
      toast.error('يرجى ملء جميع الحقول المطلوبة');
      return;
    }

    if (formData.password !== formData.confirmPassword) {
      toast.error('كلمة المرور غير متطابقة');
      return;
    }

    if (formData.password.length < 6) {
      toast.error('كلمة المرور يجب أن تكون 6 أحرف على الأقل');
      return;
    }

    setLoading(true);
    try {
      await authApi.register({
        storeName: formData.storeName,
        fullName: formData.fullName,
        phone: formData.phone,
        username: formData.username,
        password: formData.password
      });
      toast.success('تم إنشاء الحساب بنجاح! يمكنك تسجيل الدخول الآن');
      navigate('/login');
    } catch (error) {
      toast.error(error.response?.data?.message || 'فشل إنشاء الحساب');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-600 to-blue-800 flex items-center justify-center p-4">
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md p-8">
        {/* Logo */}
        <div className="text-center mb-6">
          <div className="w-16 h-16 bg-gradient-to-br from-blue-500 to-blue-700 rounded-2xl flex items-center justify-center mx-auto mb-3 shadow-lg">
            <span className="text-3xl">💰</span>
          </div>
          <h1 className="text-2xl font-bold text-gray-800">إنشاء حساب جديد</h1>
          <p className="text-gray-500 mt-1">سجل متجرك في KasserPro</p>
        </div>

        {/* Register Form */}
        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Store Name */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              اسم المتجر <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              name="storeName"
              value={formData.storeName}
              onChange={handleChange}
              className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              placeholder="مثال: مطعم السعادة"
            />
          </div>

          {/* Full Name */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              اسمك الكامل <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
              className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              placeholder="الاسم الكامل"
            />
          </div>

          {/* Phone */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              رقم الهاتف
            </label>
            <input
              type="tel"
              name="phone"
              value={formData.phone}
              onChange={handleChange}
              className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              placeholder="01xxxxxxxxx"
              dir="ltr"
            />
          </div>

          {/* Username */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              اسم المستخدم <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              name="username"
              value={formData.username}
              onChange={handleChange}
              className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              placeholder="اسم المستخدم للدخول"
              dir="ltr"
              autoComplete="username"
            />
          </div>

          {/* Password */}
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                كلمة المرور <span className="text-red-500">*</span>
              </label>
              <input
                type="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                placeholder="••••••"
                autoComplete="new-password"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                تأكيد المرور <span className="text-red-500">*</span>
              </label>
              <input
                type="password"
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                placeholder="••••••"
                autoComplete="new-password"
              />
            </div>
          </div>

          <button
            type="submit"
            disabled={loading}
            className={`w-full py-3 rounded-xl text-white font-semibold transition-all mt-2 ${
              loading
                ? 'bg-gray-400 cursor-not-allowed'
                : 'bg-gradient-to-r from-green-500 to-green-700 hover:from-green-600 hover:to-green-800 shadow-lg hover:shadow-xl'
            }`}
          >
            {loading ? (
              <span className="flex items-center justify-center gap-2">
                <svg className="animate-spin h-5 w-5" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                </svg>
                جاري إنشاء الحساب...
              </span>
            ) : (
              'إنشاء الحساب'
            )}
          </button>
        </form>

        {/* Login Link */}
        <div className="mt-6 text-center">
          <p className="text-gray-600">
            لديك حساب بالفعل؟{' '}
            <Link to="/login" className="text-blue-600 hover:text-blue-800 font-semibold">
              تسجيل الدخول
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}
