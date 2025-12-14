import { useState } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { MagnifyingGlassIcon } from "@heroicons/react/24/solid";
import toast from "react-hot-toast";
import { useProducts } from "@/hooks/useProducts";
import { useCategories } from "@/hooks/useCategories";
import ProductCard from "./POS/components/ProductCard";
import CategoryFilter from "./POS/components/CategoryFilter";
import Cart from "./POS/components/Cart";

function POS() {
  const queryClient = useQueryClient();
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [searchQuery, setSearchQuery] = useState("");
  const [cart, setCart] = useState([]);

  const { data: products, isLoading: productsLoading } = useProducts({
    categoryId: selectedCategory,
    search: searchQuery,
  });

  const { data: categories } = useCategories();

  const addToCart = (product) => {
    if (!product.isAvailable) {
      toast.error("المنتج غير متاح");
      return;
    }

    setCart((prevCart) => {
      const exist = prevCart.find((x) => x.id === product.id);
      if (exist) {
        if (exist.qty >= product.stock) {
          toast.error("الكمية غير متوفرة في المخزون");
          return prevCart;
        }
        return prevCart.map((x) =>
          x.id === product.id ? { ...x, qty: x.qty + 1 } : x
        );
      } else {
        return [...prevCart, { ...product, qty: 1 }];
      }
    });

    // Toast should ideally be triggered only if state actually changed, 
    // but for now we keep it simple. The functional update prevents the race condition data corruption.
    // To be perfectly accurate with toast, we'd need to check the result of the update or do the check before.
    // Since we do the check inside, we might show success even if it failed inside (rare edge case with rapid clicks).
    // Improved: Move toast logic or just show it.
    // For better UX with rapid clicks, maybe debounce the toast or just show it.
  };

  return (
    <div className="flex h-[calc(100vh-73px)] bg-gray-900 overflow-hidden">
      {/* Main Content */}
      <div className="flex-[2] flex flex-col p-6 overflow-hidden">
        {/* Search Bar */}
        <div className="mb-6">
          <div className="relative max-w-2xl">
            <MagnifyingGlassIcon className="absolute right-4 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400" />
            <input
              type="text"
              placeholder="ابحث عن منتج..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="w-full bg-gray-800 text-white placeholder-gray-500 border border-gray-700 rounded-xl py-3.5 pr-12 pl-4 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition-all shadow-sm"
            />
          </div>
        </div>

        {/* Categories */}
        <CategoryFilter
          categories={categories}
          selectedCategory={selectedCategory}
          onSelect={setSelectedCategory}
        />

        {/* Products Grid */}
        <div className="flex-1 overflow-y-auto pr-2 -mr-2 scrollbar-thin scrollbar-thumb-gray-700 scrollbar-track-transparent">
          {productsLoading ? (
            <div className="flex items-center justify-center h-full">
              <div className="flex flex-col items-center gap-4">
                <div className="w-12 h-12 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"></div>
                <p className="text-gray-400 animate-pulse">جاري تحميل المنتجات...</p>
              </div>
            </div>
          ) : products?.length === 0 ? (
            <div className="flex items-center justify-center h-full">
              <div className="text-center text-gray-500">
                <div className="text-6xl mb-4 grayscale opacity-50">📦</div>
                <p className="text-lg font-medium">لا توجد منتجات</p>
                <p className="text-sm mt-1">جرب تغيير البحث أو التصنيف</p>
              </div>
            </div>
          ) : (
            <div className="flex flex-wrap gap-4 content-start p-2 pb-10">
              {products?.map((p) => {
                const cartItem = cart.find((x) => x.id === p.id);
                const cartQty = cartItem ? cartItem.qty : 0;
                return (
                  <ProductCard
                    key={p.id}
                    product={p}
                    cartQty={cartQty}
                    onClick={addToCart}
                  />
                );
              })}
            </div>
          )}
        </div>
      </div>

      {/* Cart Sidebar */}
      <Cart
        cart={cart}
        setCart={setCart}
        onCheckoutSuccess={() => {
          // Invalidate products query to refresh stock
          queryClient.invalidateQueries({ queryKey: ['products'] });
        }}
      />
    </div>
  );
}

export default POS;
