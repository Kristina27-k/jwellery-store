<script lang="ts">
    import { onMount } from "svelte";
    import {
        fetchCart,
        updateQuantity,
        removeFromCart,
        clearCart,
        type CartItem,
    } from "$lib/services/cartService";
    import { goto } from "$app/navigation";

    let cartItems: CartItem[] = [];
    let loading = true;
    let error = "";

    onMount(async () => {
        await loadCart();
    });

    async function loadCart() {
        loading = true;
        try {
            cartItems = await fetchCart();
        } catch (e: any) {
            error = e.message;
        } finally {
            loading = false;
        }
    }

    async function handleUpdateQuantity(id: number, quantity: number) {
        try {
            await updateQuantity(id, quantity);
            await loadCart();
        } catch (e: any) {
            alert(e.message);
        }
    }

    async function handleRemove(id: number) {
        try {
            await removeFromCart(id);
            await loadCart();
        } catch (e: any) {
            alert(e.message);
        }
    }

    async function handleCheckout() {
        alert("Thank you for your purchase❤️ .Have a great day 💕");
      
        await clearCart();
        goto("/");
    }

    $: total = cartItems.reduce(
        (sum, item) => sum + item.price * item.quantity,
        0,
    );
</script>

<div class="cart-page">
    <div class="cart-container">
        <header>
            <h1>Your Shopping Cart</h1>
            <p>You have {cartItems.length} items in your cart.</p>
        </header>

        {#if loading}
            <div class="loading">Loading your cart...</div>
        {:else if error}
            <div class="error">{error}</div>
        {:else if cartItems.length === 0}
            <div class="empty-cart">
                <p>Your cart is empty.</p>
                <button class="btn-primary" on:click={() => goto("/")}
                    >Go Shopping</button
                >
            </div>
        {:else}
            <div class="cart-grid">
                <div class="items-list">
                    {#each cartItems as item}
                        <div class="cart-item">
                            <img src={item.imageUrl} alt={item.productName} />
                            <div class="item-details">
                                <h3>{item.productName}</h3>
                                <p class="price">Rs {item.price}</p>
                                <div class="quantity-controls">
                                    <button
                                        on:click={() =>
                                            handleUpdateQuantity(
                                                item.id,
                                                item.quantity - 1,
                                            )}
                                        disabled={item.quantity <= 1}>-</button
                                    >
                                    <span>{item.quantity}</span>
                                    <button
                                        on:click={() =>
                                            handleUpdateQuantity(
                                                item.id,
                                                item.quantity + 1,
                                            )}>+</button
                                    >
                                </div>
                            </div>
                            <div class="item-actions">
                                <p class="item-total">
                                    Rs {item.price * item.quantity}
                                </p>
                                <button
                                    class="remove-btn"
                                    on:click={() => handleRemove(item.id)}
                                    >Remove</button
                                >
                            </div>
                        </div>
                    {/each}
                </div>

                <div class="summary-card">
                    <h2>Order Summary</h2>
                    <div class="summary-row">
                        <span>Subtotal</span>
                        <span>Rs {total}</span>
                    </div>
                    <div class="summary-row">
                        <span>Shipping</span>
                        <span>Free</span>
                    </div>
                    <hr />
                    <div class="summary-row total">
                        <span>Total</span>
                        <span>Rs {total}</span>
                    </div>
                    <button
                        class="btn-primary checkout-btn"
                        on:click={handleCheckout}>Checkout Now</button
                    >
                    <button class="btn-secondary" on:click={() => goto("/")}
                        >Continue Shopping</button
                    >
                </div>
            </div>
        {/if}
    </div>
</div>

<style>
    .cart-page {
        min-height: 100vh;
        background: #fdfaf7;
        padding: 4rem 2rem;
    }

    .cart-container {
        max-width: 1200px;
        margin: 0 auto;
    }

    header {
        margin-bottom: 3rem;
        text-align: center;
    }

    h1 {
        font-size: 2.5rem;
        color: #2c2c2c;
        margin-bottom: 0.5rem;
    }

    header p {
        color: #777;
    }

    .cart-grid {
        display: grid;
        grid-template-columns: 1fr 350px;
        gap: 3rem;
    }

    .items-list {
        background: white;
        border-radius: 16px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.03);
        overflow: hidden;
    }

    .cart-item {
        display: flex;
        align-items: center;
        padding: 2rem;
        border-bottom: 1px solid #f0f0f0;
        gap: 2rem;
    }

    .cart-item img {
        width: 120px;
        height: 120px;
        object-cover: cover;
        border-radius: 12px;
    }

    .item-details {
        flex: 1;
    }

    h3 {
        font-size: 1.25rem;
        color: #333;
        margin-bottom: 0.5rem;
    }

    .price {
        color: #c9a227;
        font-weight: 600;
        margin-bottom: 1rem;
    }

    .quantity-controls {
        display: flex;
        align-items: center;
        gap: 1rem;
        background: #f5f5f5;
        width: fit-content;
        padding: 0.25rem;
        border-radius: 8px;
    }

    .quantity-controls button {
        background: white;
        border: none;
        width: 30px;
        height: 30px;
        border-radius: 6px;
        cursor: pointer;
        font-weight: bold;
        transition: all 0.2s;
    }

    .quantity-controls button:hover:not(:disabled) {
        background: #c9a227;
        color: white;
    }

    .item-actions {
        text-align: right;
    }

    .item-total {
        font-size: 1.2rem;
        font-weight: 700;
        color: #333;
        margin-bottom: 0.5rem;
    }

    .remove-btn {
        color: #ff4d4d;
        background: none;
        border: none;
        cursor: pointer;
        font-size: 0.9rem;
        text-decoration: underline;
    }

    .summary-card {
        background: white;
        padding: 2rem;
        border-radius: 16px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.03);
        height: fit-content;
    }

    h2 {
        font-size: 1.5rem;
        margin-bottom: 1.5rem;
        color: #333;
    }

    .summary-row {
        display: flex;
        justify-content: space-between;
        margin-bottom: 1rem;
        color: #555;
    }

    .total {
        font-size: 1.5rem;
        font-weight: 700;
        color: #1a1a1a;
        margin-top: 1rem;
    }

    .btn-primary {
        background: #c9a227;
        color: white;
        border: none;
        width: 100%;
        padding: 1.2rem;
        border-radius: 12px;
        font-weight: 600;
        font-size: 1.1rem;
        cursor: pointer;
        transition: all 0.2s;
        margin-top: 1.5rem;
    }

    .btn-primary:hover {
        background: #b08d20;
        transform: translateY(-2px);
    }

    .btn-secondary {
        background: transparent;
        color: #666;
        border: 1px solid #ddd;
        width: 100%;
        padding: 1rem;
        border-radius: 12px;
        margin-top: 1rem;
        cursor: pointer;
        transition: all 0.2s;
    }

    .btn-secondary:hover {
        background: #f9f9f9;
    }

    .empty-cart {
        text-align: center;
        padding: 4rem;
        background: white;
        border-radius: 16px;
    }

    .empty-cart p {
        font-size: 1.2rem;
        color: #666;
        margin-bottom: 2rem;
    }

    .loading {
        text-align: center;
        padding: 4rem;
    }
</style>
