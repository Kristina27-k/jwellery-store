<script lang="ts">
    import { auth } from "$lib/stores/authStore";
</script>

<div class="container mx-auto p-8">
    <h1 class="text-3xl font-bold mb-4">Admin Dashboard</h1>
    <p class="mb-4">Welcome, {$auth?.username}! You have Admin access.</p>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div class="p-6 bg-white rounded-lg shadow-md">
            <h2 class="text-xl font-semibold mb-2">Total Sales</h2>
            <p class="text-2xl">Rs. 12,450</p>
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

    <div class="mt-8">
        <h2 class="text-2xl font-bold mb-4">Quick Actions</h2>
        <div class="flex gap-4">
            <a
                href="/admin/items/create"
                class="px-6 py-3 bg-pink-500 text-white rounded-lg font-semibold hover:bg-pink-600 transition"
            >
                Add New Jewelry
            </a>
            <button
                class="px-6 py-3 bg-gray-200 text-gray-700 rounded-lg font-semibold hover:bg-gray-300 transition"
            >
                Manage All Items
            </button>
        </div>
    </div>
</div>
