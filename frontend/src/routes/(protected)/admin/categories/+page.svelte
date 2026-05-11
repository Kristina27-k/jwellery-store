<script lang="ts">
    import { onMount } from 'svelte';
    import { fetchCategories, deleteCategory } from '$lib/services/categoryService';
    import type { Category } from '$lib/services/categoryService';

    let categories: Category[] = [];
    let loading = true;
    let error = '';

    const loadCategories = async () => {
        try {
            loading = true;
            categories = await fetchCategories();
        } catch (e: any) {
            error = e.message || 'Failed to load categories';
        } finally {
            loading = false;
        }
    };

    const handleDelete = async (id: number) => {
        if (!confirm('Are you sure you want to delete this category?')) return;
        try {
            await deleteCategory(id);
            categories = categories.filter(c => c.id !== id);
        } catch (e: any) {
            alert(e.message || 'Failed to delete category');
        }
    };

    onMount(loadCategories);
</script>

<div class="p-8">
    <div class="flex justify-between items-center mb-8">
        <div>
            <h1 class="text-3xl font-bold text-gray-800">Categories</h1>
            <p class="text-gray-500 mt-1">Manage your jewelry categories</p>
        </div>
        <a
            href="/admin/categories/create"
            class="bg-pink-500 hover:bg-pink-600 text-white px-6 py-2.5 rounded-xl font-semibold shadow-sm shadow-pink-200 transition-all flex items-center gap-2"
        >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
            </svg>
            Add Category
        </a>
    </div>

    {#if loading}
        <div class="flex justify-center py-12">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-pink-500"></div>
        </div>
    {:else if error}
        <div class="bg-red-50 text-red-600 p-4 rounded-xl border border-red-100">{error}</div>
    {:else if categories.length === 0}
        <div class="text-center py-16 bg-white rounded-2xl border border-gray-100 shadow-sm">
            <svg class="w-16 h-16 text-gray-300 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
            </svg>
            <h3 class="text-xl font-medium text-gray-800 mb-2">No Categories Found</h3>
            <p class="text-gray-500">Get started by creating a new category.</p>
        </div>
    {:else}
        <div class="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
            <div class="overflow-x-auto">
                <table class="w-full text-left border-collapse">
                    <thead>
                        <tr class="bg-gray-50 text-gray-600 border-b border-gray-100">
                            <th class="p-4 font-semibold">ID</th>
                            <th class="p-4 font-semibold">Name</th>
                            <th class="p-4 font-semibold">Description</th>
                            <th class="p-4 font-semibold text-right">Actions</th>
                        </tr>
                    </thead>
                    <tbody class="divide-y divide-gray-100">
                        {#each categories as category}
                            <tr class="hover:bg-gray-50/50 transition-colors">
                                <td class="p-4 text-gray-500">#{category.id}</td>
                                <td class="p-4 font-medium text-gray-800">{category.cat_name}</td>
                                <td class="p-4 text-gray-600">{category.description}</td>
                                <td class="p-4 flex gap-3 justify-end">
                                    <a
                                        href={`/admin/categories/${category.id}`}
                                        class="text-blue-500 hover:bg-blue-50 px-3 py-1.5 rounded-lg transition-colors font-medium text-sm"
                                    >
                                        Edit
                                    </a>
                                    <button
                                        on:click={() => handleDelete(category.id)}
                                        class="text-red-500 hover:bg-red-50 px-3 py-1.5 rounded-lg transition-colors font-medium text-sm"
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        {/each}
                    </tbody>
                </table>
            </div>
        </div>
    {/if}
</div>
