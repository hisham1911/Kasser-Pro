import { formatCurrency, cn } from "@/lib/utils";

export default function ProductCard({ product, cartQty = 0, onClick, disabled }) {
    const isOutOfStock = product.stock <= 0;
    const isMaxQtyReached = cartQty >= product.stock;
    const isDisabled = disabled || !product.isAvailable || isOutOfStock || isMaxQtyReached;

    return (
        <button
            onClick={() => onClick(product)}
            disabled={isDisabled}
            className={cn(
                "w-[140px] bg-gray-800 rounded-xl flex flex-col items-center p-4 border border-gray-700 transition-all duration-200",
                "hover:border-blue-500 hover:shadow-lg hover:shadow-blue-500/10 hover:-translate-y-1",
                isDisabled && "opacity-50 cursor-not-allowed hover:border-gray-700 hover:shadow-none hover:translate-y-0"
            )}
        >
            <div
                className="w-16 h-16 rounded-full flex items-center justify-center mb-4 shadow-inner"
                style={{ background: product.categoryColor || "#374151" }}
            >
                <span className="text-3xl filter drop-shadow-md">
                    {product.categoryIcon || "📦"}
                </span>
            </div>

            <span className="text-sm text-gray-100 font-semibold text-center leading-tight mb-2 line-clamp-2 h-10">
                {product.name}
            </span>

            <span className="text-lg font-bold text-green-400">
                {formatCurrency(product.price)}
            </span>

            {product.stock <= 10 && product.stock > 0 && (
                <span className="text-xs text-yellow-500 mt-2 font-medium bg-yellow-500/10 px-2 py-0.5 rounded-full">
                    متبقي {product.stock}
                </span>
            )}
        </button>
    );
}
