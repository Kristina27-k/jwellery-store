import { PUBLIC_API_BASE_URL } from '$env/static/public';
import type { ApiResponse } from '$lib/types/api';
import { auth } from '$lib/stores/authStore';
import { get } from 'svelte/store';

const API_URL = `${PUBLIC_API_BASE_URL}/Payment`;

const getHeaders = () => {
    const user = get(auth);
    return {
        'Content-Type': 'application/json',
        'Authorization': user ? `Bearer ${user.token}` : ''
    };
};

export interface EsewaInitiation {
    orderId: string;
    actionUrl: string;
    fields: Record<string, string>;
}

export interface KhaltiInitiation {
    orderId: string;
    pidx: string;
    paymentUrl: string;
    expiresAt?: string | null;
    expiresIn: number;
}

const parseResponse = async <T>(response: Response): Promise<T> => {
    const result: ApiResponse<T> = await response.json();
    if (!result.isSuccess || !result.data) {
        throw new Error(result.message || 'Payment request failed');
    }

    return result.data;
};

export const initiateEsewaPayment = async (): Promise<EsewaInitiation> => {
    const response = await fetch(`${API_URL}/esewa/initiate`, {
        method: 'POST',
        headers: getHeaders()
    });

    return parseResponse<EsewaInitiation>(response);
};

export const initiateKhaltiPayment = async (): Promise<KhaltiInitiation> => {
    const response = await fetch(`${API_URL}/khalti/initiate`, {
        method: 'POST',
        headers: getHeaders()
    });

    return parseResponse<KhaltiInitiation>(response);
};

export const submitEsewaPayment = (payment: EsewaInitiation): void => {
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = payment.actionUrl;
    form.style.display = 'none';

    Object.entries(payment.fields).forEach(([name, value]) => {
        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = name;
        input.value = value;
        form.appendChild(input);
    });

    document.body.appendChild(form);
    form.submit();
};

export interface PaymentTransaction {
    id: number;
    userId: number;
    provider: string;
    orderId: string;
    providerSessionId?: string | null;
    providerTransactionId?: string | null;
    amountPaisa: number;
    amountRupees: number;
    status: string;
    clientReturnBaseUrl: string;
    rawResponse?: string | null;
    createdAt: string;
    updatedAt: string;
    completedAt?: string | null;
}

export const getAdminOrders = async (): Promise<PaymentTransaction[]> => {
    const response = await fetch(`${API_URL}/admin/orders`, {
        method: 'GET',
        headers: getHeaders()
    });

    return parseResponse<PaymentTransaction[]>(response);
};

export const getUserOrders = async (): Promise<PaymentTransaction[]> => {
    const response = await fetch(`${API_URL}/user/orders`, {
        method: 'GET',
        headers: getHeaders()
    });

    return parseResponse<PaymentTransaction[]>(response);
};
