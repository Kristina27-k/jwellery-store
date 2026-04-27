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

export const createJewelry = async (item: Omit<JewelryItem, 'id'>): Promise<JewelryItem> => {
    const response = await fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item)
    });
    const result: ApiResponse<JewelryItem> = await response.json();
    if (!result.isSuccess || !result.data) {
        throw new Error(result.message || 'Failed to create jewelry item');
    }
    return result.data;
};

export const updateJewelry = async (item: JewelryItem): Promise<boolean> => {
    const response = await fetch(API_URL, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item)
    });
    const result: ApiResponse<boolean> = await response.json();
    if (!result.isSuccess) {
        throw new Error(result.message || 'Failed to update jewelry item');
    }
    return true;
};

export const deleteJewelry = async (id: number): Promise<boolean> => {
    const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE'
    });
    const result: ApiResponse<boolean> = await response.json();
    if (!result.isSuccess) {
        throw new Error(result.message || 'Failed to delete jewelry item');
    }
    return true;
};
