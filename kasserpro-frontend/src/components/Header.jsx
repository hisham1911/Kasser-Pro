import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import {
  ShoppingCartIcon,
  Cog6ToothIcon,
  ClipboardDocumentListIcon,
  CubeIcon,
  ArrowRightOnRectangleIcon,
} from "@heroicons/react/24/solid";

function Header() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const navItems = [
    { to: "/", icon: ShoppingCartIcon, label: "نقطة البيع" },
    { to: "/orders", icon: ClipboardDocumentListIcon, label: "الطلبات" },
    { to: "/products", icon: CubeIcon, label: "المنتجات" },
  ];

  // Only show settings for Owner, Manager, SuperAdmin
  if (["Owner", "Manager", "SuperAdmin"].includes(user?.role)) {
    navItems.push({ to: "/settings", icon: Cog6ToothIcon, label: "الإعدادات" });
  }

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  // Get role display name in Arabic
  const getRoleDisplay = (role) => {
    const roles = {
      SuperAdmin: "المسؤول",
      Owner: "مالك",
      Manager: "مدير",
      Cashier: "كاشير",
    };
    return roles[role] || role;
  };

  // Get first letter for avatar
  const getInitial = () => {
    return user?.fullName?.charAt(0) || user?.username?.charAt(0) || "?";
  };

  return (
    <header className="bg-gray-900 border-b border-gray-700">
      <div className="flex items-center justify-between px-6 py-4">
        {/* Logo */}
        <div className="flex items-center gap-3">
          <span className="text-3xl">💎</span>
          <div>
            <h1 className="text-xl font-black text-white">KasserPro</h1>
            <p className="text-xs text-gray-500">{user?.storeName || "نظام نقاط البيع"}</p>
          </div>
        </div>

        {/* Navigation */}
        <nav className="flex items-center gap-2">
          {navItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                `flex items-center gap-2 px-4 py-2 rounded-lg font-semibold transition-all ${
                  isActive
                    ? "bg-blue-600 text-white"
                    : "text-gray-400 hover:text-white hover:bg-gray-800"
                }`
              }
            >
              <item.icon className="h-5 w-5" />
              <span>{item.label}</span>
            </NavLink>
          ))}
        </nav>

        {/* User */}
        <div className="flex items-center gap-4">
          <div className="flex items-center gap-3">
            <div className="text-left">
              <p className="text-sm font-bold text-white">{user?.fullName || user?.username}</p>
              <p className="text-xs text-gray-500">{getRoleDisplay(user?.role)}</p>
            </div>
            <div className="w-10 h-10 bg-blue-600 rounded-full flex items-center justify-center">
              <span className="text-white font-bold">{getInitial()}</span>
            </div>
          </div>
          
          {/* Logout Button */}
          <button
            onClick={handleLogout}
            className="flex items-center gap-2 px-3 py-2 rounded-lg text-gray-400 hover:text-white hover:bg-gray-800 transition-all"
            title="تسجيل الخروج"
          >
            <ArrowRightOnRectangleIcon className="h-5 w-5" />
          </button>
        </div>
      </div>
    </header>
  );
}

export default Header;
