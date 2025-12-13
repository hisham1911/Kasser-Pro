import { useState, useEffect } from "react";
import { PrinterIcon, TrashIcon } from "@heroicons/react/24/solid";
import { formatCurrency, cn } from "@/lib/utils";
import { ordersApi, settingsApi } from "@/services/api";
import toast from "react-hot-toast";

export default function Cart({ cart, setCart, onCheckoutSuccess }) {
    const [discount, setDiscount] = useState(0);
    const [paymentMethod, setPaymentMethod] = useState("Cash");
    const [taxEnabled, setTaxEnabled] = useState(true);
    const [taxRate, setTaxRate] = useState(0.14);
    const [isProcessing, setIsProcessing] = useState(false);

    useEffect(() => {
        const loadSettings = async () => {
            try {
                const res = await settingsApi.get();
                setTaxEnabled(res.data.taxEnabled);
                setTaxRate(res.data.taxRate / 100);
            } catch {
                // Fallback to defaults if settings fail to load
            }
        };
        loadSettings();
    }, []);

    const subtotal = cart.reduce(
        (acc, curr) => acc + parseFloat(curr.price) * curr.qty,
        0
    );

    const validDiscount = Math.min(Math.max(0, discount), subtotal);
    const taxAmount = taxEnabled ? (subtotal - validDiscount) * taxRate : 0;
    const total = subtotal - validDiscount + taxAmount;

    const updateQty = (id, delta) => {
        setCart((prev) =>
            prev.map((item) => {
                if (item.id === id) {
                    const newQty = Math.max(1, item.qty + delta);
                    if (newQty > item.stock) {
                        toast.error("الكمية غير متوفرة في المخزون");
                        return item;
                    }
                    return { ...item, qty: newQty };
                }
                return item;
            })
        );
    };

    const removeItem = (id) => {
        setCart((prev) => prev.filter((item) => item.id !== id));
    };

    const clearCart = () => {
        setCart([]);
        setDiscount(0);
    };

    const handleCheckout = async () => {
        if (cart.length === 0) return;

        setIsProcessing(true);
        const order = {
            items: cart.map((item) => ({
                productId: item.id,
                quantity: item.qty,
                priceAtTime: parseFloat(item.price),
            })),
            discount: validDiscount,
            paymentMethod: paymentMethod,
        };

        try {
            const res = await ordersApi.create(order);
            toast.success(`تم الدفع بنجاح! فاتورة: ${res.data.orderNumber}`);
            clearCart();
            onCheckoutSuccess?.();
        } catch (error) {
            const msg = error.response?.data?.message || "فشل إتمام الطلب";
            toast.error(msg);
        } finally {
            setIsProcessing(false);
        }
    };

    return (
        <div className="w-[400px] bg-gray-800 border-r border-gray-700 flex flex-col h-full shadow-2xl">
            {/* Header */}
            <div className="p-5 border-b border-gray-700 flex justify-between items-center bg-gray-800/50 backdrop-blur-sm">
                <div>
                    <h2 className="text-xl font-bold text-white flex items-center gap-2">
                        🛒 السلة
                    </h2>
                    <p className="text-sm text-gray-400 mt-1">
                        {cart.length === 0
                            ? "ابدأ بإضافة منتجات"
                            : `${cart.reduce((a, c) => a + c.qty, 0)} عنصر • ${cart.length} صنف`}
                    </p>
                </div>
                {cart.length > 0 && (
                    <button
                        onClick={clearCart}
                        className="text-red-400 text-sm font-bold hover:text-red-300 hover:bg-red-400/10 px-3 py-1.5 rounded-lg transition-colors"
                    >
                        مسح الكل
                    </button>
                )}
            </div>

            {/* Cart Items */}
            <div className="flex-1 overflow-y-auto p-4 space-y-3 scrollbar-thin scrollbar-thumb-gray-600 scrollbar-track-transparent">
                {cart.length === 0 ? (
                    <div className="h-full flex flex-col items-center justify-center text-center opacity-50">
                        <div className="text-6xl mb-4 grayscale">🛒</div>
                        <p className="text-gray-400 font-medium">السلة فارغة</p>
                        <p className="text-sm text-gray-500 mt-1">اضغط على المنتجات لإضافتها هنا</p>
                    </div>
                ) : (
                    cart.map((item) => (
                        <div
                            key={item.id}
                            className="bg-gray-700/50 rounded-xl p-3 border border-gray-700/50 hover:border-gray-600 transition-colors group"
                        >
                            <div className="flex justify-between items-start mb-3">
                                <div className="flex gap-3">
                                    <span className="text-2xl bg-gray-800 w-10 h-10 flex items-center justify-center rounded-lg">
                                        {item.categoryIcon || "📦"}
                                    </span>
                                    <div>
                                        <p className="font-bold text-white text-sm line-clamp-1">
                                            {item.name}
                                        </p>
                                        <p className="text-xs text-gray-400 mt-0.5">
                                            {formatCurrency(item.price)}
                                        </p>
                                    </div>
                                </div>
                                <button
                                    onClick={() => removeItem(item.id)}
                                    className="text-gray-500 hover:text-red-400 p-1 rounded-md hover:bg-red-400/10 transition-colors opacity-0 group-hover:opacity-100"
                                >
                                    <TrashIcon className="w-4 h-4" />
                                </button>
                            </div>

                            <div className="flex justify-between items-center">
                                <div className="flex items-center bg-gray-800 rounded-lg p-0.5 border border-gray-700">
                                    <button
                                        onClick={() => updateQty(item.id, -1)}
                                        className="w-8 h-7 flex items-center justify-center text-gray-400 hover:text-white hover:bg-gray-700 rounded-md transition-colors"
                                    >
                                        −
                                    </button>
                                    <span className="w-8 text-center text-white font-bold text-sm">
                                        {item.qty}
                                    </span>
                                    <button
                                        onClick={() => updateQty(item.id, 1)}
                                        className="w-8 h-7 flex items-center justify-center text-gray-400 hover:text-white hover:bg-gray-700 rounded-md transition-colors"
                                    >
                                        +
                                    </button>
                                </div>
                                <p className="font-bold text-green-400 tabular-nums">
                                    {formatCurrency(item.price * item.qty)}
                                </p>
                            </div>
                        </div>
                    ))
                )}
            </div>

            {/* Footer / Checkout */}
            <div className="p-5 border-t border-gray-700 bg-gray-900 space-y-4">
                {/* Discount Input */}
                <div className="flex items-center gap-3 bg-gray-800 p-2 rounded-lg border border-gray-700">
                    <label className="text-gray-400 text-sm font-medium px-1">خصم:</label>
                    <input
                        type="number"
                        min="0"
                        max={subtotal}
                        value={discount || ""}
                        onChange={(e) => setDiscount(e.target.value === "" ? 0 : parseFloat(e.target.value))}
                        className="flex-1 bg-transparent text-white text-right font-bold focus:outline-none placeholder-gray-600"
                        placeholder="0"
                    />
                    <span className="text-gray-500 text-sm">ج.م</span>
                </div>

                {/* Payment Methods */}
                <div className="grid grid-cols-3 gap-2">
                    {["Cash", "Card", "Wallet"].map((method) => (
                        <button
                            key={method}
                            onClick={() => setPaymentMethod(method)}
                            className={cn(
                                "py-2.5 rounded-lg font-bold text-xs transition-all border",
                                paymentMethod === method
                                    ? "bg-blue-600 text-white border-blue-500 shadow-lg shadow-blue-500/20"
                                    : "bg-gray-800 text-gray-400 border-gray-700 hover:bg-gray-700 hover:border-gray-600"
                            )}
                        >
                            {method === "Cash" ? "💵 كاش" : method === "Card" ? "💳 بطاقة" : "📱 محفظة"}
                        </button>
                    ))}
                </div>

                {/* Totals */}
                <div className="space-y-2 text-sm pt-2">
                    <div className="flex justify-between text-gray-400">
                        <span>المجموع الفرعي</span>
                        <span className="tabular-nums">{formatCurrency(subtotal)}</span>
                    </div>

                    {validDiscount > 0 && (
                        <div className="flex justify-between text-red-400">
                            <span>الخصم</span>
                            <span className="tabular-nums">-{formatCurrency(validDiscount)}</span>
                        </div>
                    )}

                    {taxEnabled && (
                        <div className="flex justify-between text-gray-400 group cursor-pointer" onClick={() => setTaxEnabled(!taxEnabled)}>
                            <div className="flex items-center gap-1.5">
                                <div className={cn("w-4 h-4 rounded border flex items-center justify-center transition-colors", taxEnabled ? "bg-blue-600 border-blue-600" : "border-gray-600")}>
                                    {taxEnabled && <span className="text-white text-[10px]">✓</span>}
                                </div>
                                <span>الضريبة ({(taxRate * 100).toFixed(0)}%)</span>
                            </div>
                            <span className="tabular-nums">{formatCurrency(taxAmount)}</span>
                        </div>
                    )}

                    <div className="flex justify-between text-white font-bold text-xl pt-4 border-t border-gray-700">
                        <span>الإجمالي</span>
                        <span className="text-green-400 tabular-nums">{formatCurrency(total)}</span>
                    </div>
                </div>

                {/* Checkout Button */}
                <button
                    onClick={handleCheckout}
                    disabled={cart.length === 0 || isProcessing}
                    className={cn(
                        "w-full py-4 rounded-xl font-bold flex items-center justify-center gap-3 text-lg transition-all shadow-lg",
                        cart.length > 0 && !isProcessing
                            ? "bg-green-600 hover:bg-green-500 text-white shadow-green-600/20 hover:shadow-green-500/30 hover:-translate-y-0.5"
                            : "bg-gray-800 text-gray-500 cursor-not-allowed border border-gray-700"
                    )}
                >
                    {isProcessing ? (
                        <span className="animate-pulse">جاري المعالجة...</span>
                    ) : (
                        <>
                            <PrinterIcon className="h-6 w-6" />
                            <span>إتمام الدفع</span>
                        </>
                    )}
                </button>
            </div>
        </div>
    );
}
