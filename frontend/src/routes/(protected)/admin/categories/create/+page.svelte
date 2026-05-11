<script lang="ts">
    import { goto } from '$app/navigation';
    import { createCategory } from '$lib/services/categoryService';

    let name = '';
    let description = '';
    let loading = false;
    let error = '';

    const handleSubmit = async () => {
        loading = true;
        error = '';
        try {
            await createCategory({ cat_name: name, description });
            goto('/admin/categories');
        } catch (e: any) {
            error = e.message || 'Failed to create category';
        } finally {
            loading = false;
        }
    };
</script>

<div class="p-8 max-w-2xl mx-auto">
    <div class="mb-8">
        <a href="/admin/categories" class="text-pink-500 hover:text-pink-600 flex items-center gap-2 mb-4 font-medium">
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
            Back to Categories
        </a>
        <h1 class="text-3xl font-bold text-gray-800">Create Category</h1>
        <p class="text-gray-500 mt-1">Add a new category to organize your products</p>
    </div>

    <form on:submit|preventDefault={handleSubmit} class="bg-white p-8 rounded-2xl shadow-sm border border-gray-100">
        {#if error}
            <div class="bg-red-50 text-red-600 p-4 rounded-xl border border-red-100 mb-6">{error}</div>
        {/if}

        <div class="space-y-6">
            <div>
                <label for="name" class="block text-sm font-semibold text-gray-700 mb-2">Category Name</label>
                <input
                    id="name"
                    type="text"
                    bind:value={name}
                    required
                    class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:border-pink-500 focus:ring-2 focus:ring-pink-200 transition-colors outline-none"
                    placeholder="e.g. Necklaces"
                />
            </div>

            <div>
                <label for="description" class="block text-sm font-semibold text-gray-700 mb-2">Description</label>
                <textarea
                    id="description"
                    bind:value={description}
                    rows="4"
                    class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:border-pink-500 focus:ring-2 focus:ring-pink-200 transition-colors outline-none resize-none"
                    placeholder="A brief description of this category..."
                ></textarea>
            </div>

            <button
                type="submit"
                disabled={loading}
                class="w-full bg-pink-500 hover:bg-pink-600 text-white font-semibold py-3.5 rounded-xl shadow-sm shadow-pink-200 transition-all disabled:opacity-70 disabled:cursor-not-allowed"
            >
                {loading ? 'Creating...' : 'Create Category'}
            </button>
        </div>
    </form>
</div>
