import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import toast from 'react-hot-toast';

export default function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!username || !password) {
      toast.error('يرجى ملء جميع الحقول');
      return;
    }

    setLoading(true);
    try {
      await login(username, password);
      toast.success('تم تسجيل الدخول بنجاح');
      navigate('/');
    } catch (error) {
      toast.error(error.message || 'فشل تسجيل الدخول');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-600 to-blue-800 flex items-center justify-center p-4">
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md p-8">
        {/* Logo */}
        <div className="text-center mb-8">
          <div className="w-20 h-20 bg-gradient-to-br from-blue-500 to-blue-700 rounded-2xl flex items-center justify-center mx-auto mb-4 shadow-lg">
            <span className="text-4xl">💰</span>
          </div>
          <h1 className="text-3xl font-bold text-gray-800">KasserPro</h1>
          <p className="text-gray-500 mt-2">نظام نقاط البيع الذكي</p>
        </div>

        {/* Login Form */}
        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              اسم المستخدم
            </label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all"
              placeholder="أدخل اسم المستخدم"
              autoComplete="username"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              كلمة المرور
            </label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all"
              placeholder="أدخل كلمة المرور"
              autoComplete="current-password"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className={`w-full py-3 rounded-xl text-white font-semibold transition-all ${
              loading
                ? 'bg-gray-400 cursor-not-allowed'
                : 'bg-gradient-to-r from-blue-500 to-blue-700 hover:from-blue-600 hover:to-blue-800 shadow-lg hover:shadow-xl'
            }`}
          >
            {loading ? (
              <span className="flex items-center justify-center gap-2">
                <svg className="animate-spin h-5 w-5" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                </svg>
                جاري تسجيل الدخول...
              </span>
            ) : (
              'تسجيل الدخول'
            )}
          </button>
        </form>

        {/* Register Link */}
        <div className="mt-6 text-center">
          <p className="text-gray-600">
            ليس لديك حساب؟{' '}
            <Link to="/register" className="text-blue-600 hover:text-blue-800 font-semibold">
              سجل متجرك الآن
            </Link>
          </p>
        </div>

        {/* Demo Credentials */}
        <div className="mt-6 p-4 bg-gray-50 rounded-xl">
          <p className="text-sm text-gray-600 text-center">
            <span className="font-semibold">للتجربة:</span> admin / admin123
          </p>
        </div>
      </div>
    </div>
  );
}
