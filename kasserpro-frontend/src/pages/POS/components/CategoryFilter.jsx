import { cn } from "@/lib/utils";

export default function CategoryFilter({ categories, selectedCategory, onSelect }) {
    return (
        <div className="flex gap-2 mb-6 overflow-x-auto pb-2 scrollbar-hide">
            <button
                onClick={() => onSelect(null)}
                className={cn(
                    "px-5 py-2.5 rounded-full font-bold whitespace-nowrap transition-all text-sm shadow-sm",
                    selectedCategory === null
                        ? "bg-blue-600 text-white shadow-blue-500/25"
                        : "bg-gray-800 text-gray-400 hover:bg-gray-700 hover:text-gray-200"
                )}
            >
                🌟 الكل
            </button>

            {categories?.map((cat) => (
                <button
                    key={cat.id}
                    onClick={() => onSelect(cat.id)}
                    className={cn(
                        "px-5 py-2.5 rounded-full font-bold whitespace-nowrap transition-all text-sm shadow-sm flex items-center gap-2",
                        selectedCategory === cat.id
                            ? "text-white ring-2 ring-white/20"
                            : "text-gray-400 hover:text-gray-200 bg-gray-800 hover:bg-gray-700"
                    )}
                    style={{
                        backgroundColor: selectedCategory === cat.id ? cat.color : undefined,
                    }}
                >
                    <span>{cat.icon}</span>
                    <span>{cat.name}</span>
                </button>
            ))}
        </div>
    );
}
