<script lang="ts">
    import { auth } from '$lib/stores/authStore';
</script>

<div class="container mx-auto p-8">
    <h1 class="text-3xl font-bold mb-4">Admin Dashboard</h1>
    <p class="mb-4">Welcome, {$auth?.username}! You have Admin access.</p>
    
    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div class="p-6 bg-white rounded-lg shadow-md">
            <h2 class="text-xl font-semibold mb-2">Total Sales</h2>
            <p class="text-2xl">$12,450</p>
        </div>
        <div class="p-6 bg-white rounded-lg shadow-md">
            <h2 class="text-xl font-semibold mb-2">Orders</h2>
            <p class="text-2xl">45</p>
        </div>
        <div class="p-6 bg-white rounded-lg shadow-md">
            <h2 class="text-xl font-semibold mb-2">Customers</h2>
            <p class="text-2xl">120</p>
        </div>
    </div>
</div>
