import { PUBLIC_API_BASE_URL } from '$env/static/public';
import type { JewelryItem } from '$lib/types/jewelry';
import type { ApiResponse } from '$lib/types/api';

const API_URL = `${PUBLIC_API_BASE_URL}/Jewelry`;

export const fetchJewelry = async (): Promise<JewelryItem[]> => {
    const response = await fetch(API_URL);
    const result: ApiResponse<JewelryItem[]> = await response.json();
    
    if (!result.isSuccess) {
        throw new Error(result.message || 'Failed to fetch jewelry items');
    }
    return result.data || [];
};

export const fetchJewelryById = async (id: number): Promise<JewelryItem> => {
    const response = await fetch(`${API_URL}/${id}`);
    const result: ApiResponse<JewelryItem> = await response.json();
    
    if (!result.isSuccess || !result.data) {
        throw new Error(result.message || `Failed to fetch jewelry item with id ${id}`);
    }
    return result.data;
};
