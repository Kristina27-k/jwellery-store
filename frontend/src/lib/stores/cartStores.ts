import { writable } from "svelte/store";

export const cart = writable<any[]>([]);

export function addToCart(item: any) {
	cart.update((items) => {
		const existing = items.find((i) => i.id === item.id);

		if (existing) {
			existing.quantity += 1;
			return [...items];
		}

		return [...items, { ...item, quantity: 1 }];
	});
}