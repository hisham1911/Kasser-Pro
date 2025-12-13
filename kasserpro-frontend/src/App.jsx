import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Toaster } from "react-hot-toast";
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
                        <ProtectedRoute allowedRoles={["Owner", "Manager", "SuperAdmin"]}>
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
            duration: 2000,
            style: {
              background: "#1f2937",
              color: "#fff",
              border: "1px solid #374151",
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
        />
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
