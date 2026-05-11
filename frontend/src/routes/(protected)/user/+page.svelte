<script lang="ts">
    import { onMount } from "svelte";
    import { auth } from "$lib/stores/authStore";
    import { fetchCart, updateQuantity, removeFromCart } from "$lib/services/cartService";
    import type { CartItem } from "$lib/services/cartService";

    let cartItems: CartItem[] = [];
    let loading = false;
    let error = "";

    async function loadCart() {
        loading = true;
        error = "";
        try {
            cartItems = await fetchCart();
        } catch (e: any) {
            error = e.message || "Unable to load cart items.";
        } finally {
            loading = false;
        }
    }

    async function handleQuantityChange(item: CartItem, amount: number) {
        const quantity = item.quantity + amount;
        if (quantity <= 0) return;
        try {
            await updateQuantity(item.id, quantity);
            await loadCart();
        } catch (e: any) {
            error = e.message || "Unable to update quantity.";
        }
    }

    async function handleRemove(item: CartItem) {
        if (!confirm(`Remove ${item.productName} from your cart?`)) return;
        try {
            await removeFromCart(item.id);
            await loadCart();
        } catch (e: any) {
            error = e.message || "Unable to remove item.";
        }
    }

    $: cartTotal = cartItems.reduce((sum, item) => sum + item.totalPrice, 0);

    onMount(loadCart);
</script>

<div class="container mx-auto p-8">
    <h1 class="text-3xl font-bold mb-4">My Account</h1>
    <p class="mb-4">Hello, {$auth?.username}! This is your personal area.</p>

    <div class="grid gap-6 lg:grid-cols-[1.2fr_0.8fr]">
        <section class="bg-white p-6 rounded-xl shadow-sm">
            <h2 class="text-2xl font-semibold mb-4">Profile Information</h2>
            <div class="space-y-3 text-gray-700">
                <p><strong>Username:</strong> {$auth?.username}</p>
                <p><strong>Email:</strong> {$auth?.email}</p>
                <p><strong>Role:</strong> {$auth?.role}</p>
            </div>
        </section>

        <section class="bg-white p-6 rounded-xl shadow-sm">
            <h2 class="text-2xl font-semibold mb-4">My Cart Summary</h2>
            {#if loading}
                <p>Loading your cart...</p>
            {:else if error}
                <div class="error-message">{error}</div>
            {:else if cartItems.length === 0}
                <p>Your cart is empty. Add something from the shop to get started.</p>
            {:else}
                <ul class="cart-list">
                    {#each cartItems as item}
                        <li class="cart-item">
                            <div>
                                <p class="item-name">{item.productName}</p>
                                <p class="item-meta">Rs. {item.price.toFixed(2)} × {item.quantity}</p>
                            </div>
                            <div class="item-actions">
                                <button on:click={() => handleQuantityChange(item, -1)}>-</button>
                                <span>{item.quantity}</span>
                                <button on:click={() => handleQuantityChange(item, 1)}>+</button>
                                <button class="remove" on:click={() => handleRemove(item)}>Remove</button>
                            </div>
                        </li>
                    {/each}
                </ul>
                <div class="cart-total">
                    <span>Total</span>
                    <strong>Rs. {cartTotal.toFixed(2)}</strong>
                </div>
                <a href="/cart" class="btn-primary">View Full Cart</a>
            {/if}
        </section>
    </div>
</div>

<style>
    .cart-list {
        display: grid;
        gap: 1rem;
        margin-bottom: 1.5rem;
    }

    .cart-item {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 1rem;
        padding: 1rem;
        border: 1px solid #e5e7eb;
        border-radius: 14px;
    }

    .item-name {
        font-weight: 600;
        margin-bottom: 0.25rem;
    }

    .item-meta {
        color: #6b7280;
        font-size: 0.95rem;
    }

    .item-actions {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        flex-wrap: wrap;
    }

    .item-actions button {
        background: #f3f4f6;
        border: 1px solid #d1d5db;
        color: #111827;
        padding: 0.5rem 0.85rem;
        border-radius: 10px;
        cursor: pointer;
        font-weight: 700;
    }

    .item-actions button.remove {
        background: #fee2e2;
        border-color: #fecaca;
        color: #991b1b;
    }

    .cart-total {
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-size: 1.1rem;
        font-weight: 700;
        margin-bottom: 1rem;
    }

    .btn-primary {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        background: #c9a227;
        color: white;
        padding: 0.95rem 1.2rem;
        border-radius: 12px;
        transition: transform 0.15s ease, opacity 0.15s ease;
        text-decoration: none;
    }

    .btn-primary:hover {
        transform: translateY(-1px);
        opacity: 0.95;
    }

    .error-message {
        background: #fef2f2;
        border: 1px solid #fecaca;
        color: #991b1b;
        padding: 1rem;
        border-radius: 12px;
    }
</style>
