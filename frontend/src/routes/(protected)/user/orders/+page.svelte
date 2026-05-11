<script lang="ts">
    import { onMount } from 'svelte';
    import { getUserOrders } from '$lib/services/paymentService';
    import type { PaymentTransaction } from '$lib/services/paymentService';

    let orders: PaymentTransaction[] = [];
    let loading = true;
    let error = '';

    const loadOrders = async () => {
        try {
            loading = true;
            orders = await getUserOrders();
        } catch (e: any) {
            error = e.message || 'Failed to load orders';
        } finally {
            loading = false;
        }
    };

    onMount(loadOrders);
    
    function formatDate(dateStr: string) {
        return new Date(dateStr).toLocaleString('en-US', {
            year: 'numeric', month: 'short', day: 'numeric',
            hour: '2-digit', minute: '2-digit'
        });
    }

    function getStatusBadgeClass(status: string) {
        const s = status.toLowerCase();
        if (s === 'completed' || s === 'success') return 'bg-green-100 text-green-800 border-green-200';
        if (s === 'failed' || s === 'canceled') return 'bg-red-100 text-red-800 border-red-200';
        if (s === 'initiated') return 'bg-yellow-100 text-yellow-800 border-yellow-200';
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
</script>

<div class="p-8">
    <div class="mb-8">
        <h1 class="text-3xl font-bold text-gray-800">My Orders</h1>
        <p class="text-gray-500 mt-1">Review your past purchases and their status</p>
    </div>

    {#if loading}
        <div class="flex justify-center py-12">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
        </div>
    {:else if error}
        <div class="bg-red-50 text-red-600 p-4 rounded-xl border border-red-100">{error}</div>
    {:else if orders.length === 0}
        <div class="text-center py-16 bg-white rounded-2xl border border-gray-100 shadow-sm">
            <svg class="w-16 h-16 text-gray-300 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
            </svg>
            <h3 class="text-xl font-medium text-gray-800 mb-2">No Orders Yet</h3>
            <p class="text-gray-500 mb-6">Looks like you haven't made any purchases yet.</p>
            <a href="/" class="inline-flex bg-blue-500 hover:bg-blue-600 text-white px-6 py-2.5 rounded-xl font-semibold shadow-sm shadow-blue-200 transition-all">
                Start Shopping
            </a>
        </div>
    {:else}
        <div class="grid gap-4">
            {#each orders as order}
                <div class="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 flex flex-col md:flex-row md:items-center justify-between gap-4">
                    <div>
                        <div class="flex items-center gap-3 mb-2">
                            <span class="font-medium text-gray-800">Order #{order.orderId.substring(0, 8)}...</span>
                            <span class="px-2.5 py-1 text-xs font-medium rounded-full border {getStatusBadgeClass(order.status)}">
                                {order.status}
                            </span>
                        </div>
                        <p class="text-gray-500 text-sm mb-1">Placed on {formatDate(order.createdAt)}</p>
                        <p class="text-sm">
                            <span class="text-gray-500">Paid via</span>
                            <span class="capitalize font-medium {order.provider.toLowerCase() === 'esewa' ? 'text-green-600' : order.provider.toLowerCase() === 'cod' ? 'text-gray-600' : 'text-purple-600'}">
                                {order.provider.toUpperCase() === 'COD' ? 'Cash on Delivery' : order.provider}
                            </span>
                        </p>
                    </div>
                    <div class="text-left md:text-right">
                        <p class="text-gray-500 text-sm">Total Amount</p>
                        <p class="text-xl font-bold text-gray-800">Rs. {order.amountRupees.toFixed(2)}</p>
                    </div>
                </div>
            {/each}
        </div>
    {/if}
</div>
