import { PUBLIC_API_BASE_URL } from '$env/static/public';
import type { ApiResponse } from '$lib/types/api';
import { auth } from '$lib/stores/authStore';
import { get } from 'svelte/store';

const API_URL = `${PUBLIC_API_BASE_URL}/Category`;

export interface Category {
    id: number;
    cat_name: string;
    description: string;
}

export interface CategoryInput {
    id?: number;
    cat_name: string;
    description: string;
}

const getHeaders = (requireAuth = false) => {
    const headers: Record<string, string> = {
        'Content-Type': 'application/json'
    };

    if (requireAuth) {
        const user = get(auth);
        if (user) {
            headers['Authorization'] = `Bearer ${user.token}`;
        }
    }

    return headers;
};

const parseResponse = async <T>(response: Response): Promise<T> => {
    const result: ApiResponse<T> = await response.json();
    if (!result.isSuccess) {
        throw new Error(result.message || 'Request failed');
    }
    // For delete, the data might be true/false or null, we can return as is or return result.data
    // but the generic type will handle it.
    return result.data as T;
};

export const fetchCategories = async (): Promise<Category[]> => {
    const response = await fetch(API_URL, {
        method: 'GET',
        headers: getHeaders()
    });
    return parseResponse<Category[]>(response);
};

export const fetchCategoryById = async (id: number): Promise<Category> => {
    const response = await fetch(`${API_URL}/${id}`, {
        method: 'GET',
        headers: getHeaders()
    });
    return parseResponse<Category>(response);
};

export const createCategory = async (category: CategoryInput): Promise<Category> => {
    const response = await fetch(API_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(category)
    });
    return parseResponse<Category>(response);
};

export const updateCategory = async (category: CategoryInput): Promise<Category> => {
    const response = await fetch(API_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(category)
    });
    return parseResponse<Category>(response);
};

export const deleteCategory = async (id: number): Promise<boolean> => {
    const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders(true)
    });
    return parseResponse<boolean>(response);
};
