import { writable } from 'svelte/store';
import { browser } from '$app/environment';
import type { User } from '$lib/types/user';

// Helper to get/set cookies
const getCookie = (name: string) => {
    if (!browser) return null;
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop()?.split(';').shift();
    return null;
};

const setCookie = (name: string, value: string, days = 7) => {
    if (!browser) return;
    const expires = new Date(Date.now() + days * 864e5).toUTCString();
    document.cookie = `${name}=${encodeURIComponent(value)}; expires=${expires}; path=/; SameSite=Strict`;
};

const removeCookie = (name: string) => {
    if (!browser) return;
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
};

const initialState: User | null = browser ? JSON.parse(localStorage.getItem('user') || getCookie('user') || 'null') : null;

export const auth = writable<User | null>(initialState);

if (browser) {
    auth.subscribe((user) => {
        if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            setCookie('user', JSON.stringify(user));
        } else {
            localStorage.removeItem('user');
            removeCookie('user');
        }
    });
}

export const logout = () => {
    auth.set(null);
};
