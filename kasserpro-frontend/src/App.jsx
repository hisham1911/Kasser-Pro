import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Toaster, toast } from "react-hot-toast";
import { AuthProvider } from "./context/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import Header from "./components/Header";
import Login from "./pages/Login";
import Register from "./pages/Register";
import POS from "./pages/POS";
import Orders from "./pages/Orders";
import Products from "./pages/Products";
import Settings from "./pages/Settings";

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          {/* Public Routes */}
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          {/* Protected Routes */}
          <Route
            path="/*"
            element={
              <ProtectedRoute>
                <div className="min-h-screen bg-gray-900">
                  <Header />
                  <Routes>
                    <Route path="/" element={<POS />} />
                    <Route path="/orders" element={<Orders />} />
                    <Route path="/products" element={<Products />} />
                    <Route
                      path="/settings"
                      element={
                        <ProtectedRoute
                          allowedRoles={["Owner", "Manager", "SuperAdmin"]}
                        >
                          <Settings />
                        </ProtectedRoute>
                      }
                    />
                  </Routes>
                </div>
              </ProtectedRoute>
            }
          />
        </Routes>
        <Toaster
          position="top-center"
          toastOptions={{
            duration: 3000,
            style: {
              background: "#1f2937",
              color: "#fff",
              border: "1px solid #374151",
              padding: "16px",
              borderRadius: "12px",
            },
            success: {
              iconTheme: {
                primary: "#22c55e",
                secondary: "#fff",
              },
            },
            error: {
              iconTheme: {
                primary: "#ef4444",
                secondary: "#fff",
              },
            },
          }}
        >
          {(t) => (
            <div
              style={{
                opacity: t.visible ? 1 : 0,
                transform: t.visible ? "translateY(0)" : "translateY(-20px)",
                transition: "all 0.2s",
              }}
              className="flex items-center gap-3 bg-gray-800 text-white px-4 py-3 rounded-xl shadow-lg border border-gray-700 min-w-[300px]"
            >
              {/* Icon */}
              <div className="shrink-0">
                {t.type === "success" && "✅"}
                {t.type === "error" && "❌"}
                {t.type === "loading" && "⏳"}
                {t.type === "blank" && "ℹ️"}
              </div>

              {/* Message */}
              <div className="flex-1 text-sm font-medium">
                {typeof t.message === "function" ? t.message(t) : t.message}
              </div>

              {/* Close Button */}
              <button
                onClick={() => toast.dismiss(t.id)}
                className="shrink-0 p-1 hover:bg-gray-700 rounded-full transition-colors text-gray-400 hover:text-white"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                  className="w-5 h-5"
                >
                  <path d="M6.28 5.22a.75.75 0 00-1.06 1.06L8.94 10l-3.72 3.72a.75.75 0 101.06 1.06L10 11.06l3.72 3.72a.75.75 0 101.06-1.06L11.06 10l3.72-3.72a.75.75 0 00-1.06-1.06L10 8.94 6.28 5.22z" />
                </svg>
              </button>
            </div>
          )}
        </Toaster>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
