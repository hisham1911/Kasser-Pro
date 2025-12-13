import { useQuery } from "@tanstack/react-query";
import { categoriesApi } from "@/services/api";

export function useCategories() {
    return useQuery({
        queryKey: ["categories"],
        queryFn: async () => {
            const { data } = await categoriesApi.getAll();
            return data;
        },
    });
}
