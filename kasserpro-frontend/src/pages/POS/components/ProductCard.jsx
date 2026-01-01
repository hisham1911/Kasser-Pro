import { formatCurrency, cn } from "@/lib/utils";

export default function ProductCard({
  product,
  cartQty = 0,
  onClick,
  disabled,
}) {
  const isOutOfStock = product.stock <= 0;
  const isMaxQtyReached = cartQty >= product.stock;
  const isDisabled =
    disabled || !product.isAvailable || isOutOfStock || isMaxQtyReached;

  return (
    <button
      onClick={() => onClick(product)}
      disabled={isDisabled}
      className={cn(
        "w-[120px] bg-gray-800 rounded-xl flex flex-col items-center p-3 border border-gray-700 relative z-0",
        "transition duration-200 ease-out",
        "hover:border-blue-500 hover:shadow-lg hover:shadow-blue-500/20 hover:-translate-y-1 hover:z-50",
        isDisabled &&
          "opacity-50 cursor-not-allowed hover:border-gray-700 hover:shadow-none hover:translate-y-0 hover:z-0"
      )}
    >
      <div
        className="w-12 h-12 rounded-full flex items-center justify-center mb-2 shadow-inner"
        style={{ background: product.categoryColor || "#374151" }}
      >
        <span className="text-2xl filter drop-shadow-md">
          {product.categoryIcon || "📦"}
        </span>
      </div>

      <span className="text-xs text-gray-100 font-semibold text-center leading-tight mb-1 line-clamp-2 h-8">
        {product.name}
      </span>

      <span className="text-base font-bold text-green-400">
        {formatCurrency(product.price)}
      </span>

      {product.stock <= 10 && product.stock > 0 && (
        <span className="text-[10px] text-yellow-500 mt-1 font-medium bg-yellow-500/10 px-1.5 py-0.5 rounded-full">
          متبقي {product.stock}
        </span>
      )}
    </button>
  );
}
