<script lang="ts">
    import { onMount } from 'svelte';
    import { getAdminOrders } from '$lib/services/paymentService';
    import type { PaymentTransaction } from '$lib/services/paymentService';

    let orders: PaymentTransaction[] = [];
    let loading = true;
    let error = '';

    const loadOrders = async () => {
        try {
            loading = true;
            orders = await getAdminOrders();
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
        <h1 class="text-3xl font-bold text-gray-800">All Orders</h1>
        <p class="text-gray-500 mt-1">View and manage all customer orders and transactions</p>
    </div>

    {#if loading}
        <div class="flex justify-center py-12">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-pink-500"></div>
        </div>
    {:else if error}
        <div class="bg-red-50 text-red-600 p-4 rounded-xl border border-red-100">{error}</div>
    {:else if orders.length === 0}
        <div class="text-center py-16 bg-white rounded-2xl border border-gray-100 shadow-sm">
            <svg class="w-16 h-16 text-gray-300 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg>
            <h3 class="text-xl font-medium text-gray-800 mb-2">No Orders Found</h3>
            <p class="text-gray-500">There are no transactions recorded yet.</p>
        </div>
    {:else}
        <div class="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
            <div class="overflow-x-auto">
                <table class="w-full text-left border-collapse">
                    <thead>
                        <tr class="bg-gray-50 text-gray-600 border-b border-gray-100">
                            <th class="p-4 font-semibold">Order ID</th>
                            <th class="p-4 font-semibold">User ID</th>
                            <th class="p-4 font-semibold">Provider</th>
                            <th class="p-4 font-semibold">Amount</th>
                            <th class="p-4 font-semibold">Date</th>
                            <th class="p-4 font-semibold">Status</th>
                        </tr>
                    </thead>
                    <tbody class="divide-y divide-gray-100">
                        {#each orders as order}
                            <tr class="hover:bg-gray-50/50 transition-colors">
                                <td class="p-4 font-medium text-gray-800">
                                    <span class="block truncate w-32" title={order.orderId}>{order.orderId}</span>
                                </td>
                                <td class="p-4 text-gray-500">#{order.userId}</td>
                                <td class="p-4">
                                    <span class="capitalize font-medium {order.provider.toLowerCase() === 'esewa' ? 'text-green-600' : order.provider.toLowerCase() === 'cod' ? 'text-gray-600' : 'text-purple-600'}">
                                        {order.provider.toUpperCase() === 'COD' ? 'Cash on Delivery' : order.provider}
                                    </span>
                                </td>
                                <td class="p-4 font-medium text-gray-800">Rs. {order.amountRupees.toFixed(2)}</td>
                                <td class="p-4 text-gray-500 text-sm">{formatDate(order.createdAt)}</td>
                                <td class="p-4">
                                    <span class="px-2.5 py-1 text-xs font-medium rounded-full border {getStatusBadgeClass(order.status)}">
                                        {order.status}
                                    </span>
                                </td>
                            </tr>
                        {/each}
                    </tbody>
                </table>
            </div>
        </div>
    {/if}
</div>
