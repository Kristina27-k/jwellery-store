import { PUBLIC_API_BASE_URL } from '$env/static/public';
import type { User } from '$lib/types/user';
import type { ApiResponse } from '$lib/types/api';

const API_URL = `${PUBLIC_API_BASE_URL}/Auth`;

export const register = async (username: string, email: string, password: string): Promise<User> => {
    const response = await fetch(`${API_URL}/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, email, password })
    });

    const result: ApiResponse<User> = await response.json();

    if (!result.isSuccess) {
        throw new Error(result.message || 'Registration failed');
    }

    return result.data as User;
};

export const login = async (email: string, password: string): Promise<User> => {
    const response = await fetch(`${API_URL}/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });

    const result: ApiResponse<User> = await response.json();

    if (!result.isSuccess) {
        throw new Error(result.message || 'Login failed');
    }

    return result.data as User;
};
