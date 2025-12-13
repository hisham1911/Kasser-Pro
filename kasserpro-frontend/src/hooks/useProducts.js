import { useQuery } from "@tanstack/react-query";
import { productsApi } from "@/services/api";

export function useProducts({ categoryId, search } = {}) {
    return useQuery({
        queryKey: ["products", { categoryId, search }],
        queryFn: async () => {
            const { data } = await productsApi.getAll({ categoryId, search });
            return data;
        },
    });
}
