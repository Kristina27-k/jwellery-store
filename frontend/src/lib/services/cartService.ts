import { PUBLIC_API_BASE_URL } from '$env/static/public';
import type { ApiResponse } from '$lib/types/api';
import { auth } from '$lib/stores/authStore';
import { get } from 'svelte/store';

const API_URL = `${PUBLIC_API_BASE_URL}/Cart`;

const getHeaders = () => {
    const user = get(auth);
    return {
        'Content-Type': 'application/json',
        'Authorization': user ? `Bearer ${user.token}` : ''
    };
};

export interface CartItem {
    id: number;
    jewelryItemId: number;
    productName: string;
    price: number;
    imageUrl: string;
    quantity: number;
    totalPrice: number;
}

export const fetchCart = async (): Promise<CartItem[]> => {
    const response = await fetch(API_URL, { headers: getHeaders() });
    const result: ApiResponse<CartItem[]> = await response.json();
    if (!result.isSuccess) throw new Error(result.message);
    return result.data || [];
};

export const addToCart = async (jewelryItemId: number, quantity: number = 1): Promise<void> => {
    const response = await fetch(`${API_URL}/add`, {
        method: 'POST',
        headers: getHeaders(),
        body: JSON.stringify({ jewelryItemId, quantity })
    });
    const result: ApiResponse<any> = await response.json();
    if (!result.isSuccess) throw new Error(result.message);
};

export const updateQuantity = async (cartItemId: number, quantity: number): Promise<void> => {
    const response = await fetch(`${API_URL}/update`, {
        method: 'PUT',
        headers: getHeaders(),
        body: JSON.stringify({ cartItemId, quantity })
    });
    const result: ApiResponse<any> = await response.json();
    if (!result.isSuccess) throw new Error(result.message);
};

export const removeFromCart = async (id: number): Promise<void> => {
    const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const result: ApiResponse<any> = await response.json();
    if (!result.isSuccess) throw new Error(result.message);
};

export const clearCart = async (): Promise<void> => {
    const response = await fetch(`${API_URL}/clear`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const result: ApiResponse<any> = await response.json();
    if (!result.isSuccess) throw new Error(result.message);
};
