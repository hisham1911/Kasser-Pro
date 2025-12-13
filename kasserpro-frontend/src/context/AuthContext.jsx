import { createContext, useContext, useState, useEffect } from 'react';
import { authApi } from '../services/api';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [loading, setLoading] = useState(true);

  // Check if user is authenticated on mount
  useEffect(() => {
    const checkAuth = async () => {
      if (token) {
        try {
          const response = await authApi.me();
          setUser(response.data);
        } catch (error) {
          // Token invalid or expired
          localStorage.removeItem('token');
          setToken(null);
          setUser(null);
        }
      }
      setLoading(false);
    };
    checkAuth();
  }, [token]);

  const login = async (username, password) => {
    const response = await authApi.login(username, password);
    const { token: newToken, user: userData } = response.data;
    
    localStorage.setItem('token', newToken);
    setToken(newToken);
    setUser(userData);
    
    return userData;
  };

  const logout = () => {
    localStorage.removeItem('token');
    setToken(null);
    setUser(null);
  };

  const value = {
    user,
    token,
    loading,
    isAuthenticated: !!user,
    login,
    logout
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
