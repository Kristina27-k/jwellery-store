import { writable } from 'svelte/store';
import { browser } from '$app/environment';
import type { User } from '$lib/types/user';

const initialState: User | null = browser ? JSON.parse(localStorage.getItem('user') || 'null') : null;

export const auth = writable<User | null>(initialState);

if (browser) {
    auth.subscribe((user) => {
        if (user) {
            localStorage.setItem('user', JSON.stringify(user));
        } else {
            localStorage.removeItem('user');
        }
    });
}

export const logout = () => {
    auth.set(null);
};
