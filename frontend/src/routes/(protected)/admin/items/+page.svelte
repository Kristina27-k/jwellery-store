<script lang="ts">
    import { onMount } from "svelte";
    import { goto } from "$app/navigation";
    import type { JewelryItem } from "$lib/types/jewelry";
    import { fetchJewelry, deleteJewelry } from "$lib/services/jewelryService";

    let items: JewelryItem[] = [];
    let loading = false;
    let error = "";

    async function loadItems() {
        loading = true;
        error = "";

        try {
            items = await fetchJewelry();
        } catch (e: any) {
            error = e.message || "Unable to load jewelry items.";
        } finally {
            loading = false;
        }
    }

    async function handleDelete(id: number) {
        if (!confirm("Delete this item permanently?")) return;
        try {
            await deleteJewelry(id);
            await loadItems();
        } catch (e: any) {
            error = e.message || "Unable to delete item.";
        }
    }

    onMount(() => {
        loadItems();
    });
</script>

<div class="admin-container">
    <div class="header-row">
        <div>
            <h1>Manage Jewelry Items</h1>
            <p class="subtitle">Create, edit, and delete products for your store.</p>
        </div>
        <button class="btn-primary" on:click={() => goto("/admin/items/create")}>Add New Item</button>
    </div>

    {#if loading}
        <div class="status-card">Loading items...</div>
    {:else if error}
        <div class="status-card error">{error}</div>
    {:else if items.length === 0}
        <div class="status-card">No jewelry items found. Start by adding a new product.</div>
    {:else}
        <div class="table-wrapper">
            <table>
                <thead>
                    <tr>
                        <th>Preview</th>
                        <th>Name</th>
                        <th>Price</th>
                        <th>Category</th>
                        <th>Description</th>
                        <th class="actions">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {#each items as item}
                        <tr>
                            <td>
                                <img src={item.imageUrl} alt={item.name} />
                            </td>
                            <td>{item.name}</td>
                            <td>Rs. {item.price.toFixed(2)}</td>
                            <td>{item.categoryName ?? `Category ${item.categoryId}`}</td>
                            <td>{item.description}</td>
                            <td class="actions">
                                <button class="btn-secondary" on:click={() => goto(`/admin/items/${item.id}/edit`)}>
                                    Edit
                                </button>
                                <button class="btn-danger" on:click={() => handleDelete(item.id)}>
                                    Delete
                                </button>
                            </td>
                        </tr>
                    {/each}
                </tbody>
            </table>
        </div>
    {/if}
</div>

<style>
    .admin-container {
        max-width: 1200px;
        margin: 0 auto;
        padding: 2rem;
    }

    .header-row {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 1rem;
        margin-bottom: 1.5rem;
    }

    h1 {
        font-size: 2.25rem;
        margin-bottom: 0.25rem;
    }

    .subtitle {
        color: #666;
    }

    .status-card {
        background: white;
        padding: 1.5rem;
        border-radius: 16px;
        border: 1px solid #e5e7eb;
        color: #333;
    }

    .status-card.error {
        border-color: #fca5a5;
        background: #fff1f2;
        color: #b91c1c;
    }

    .table-wrapper {
        overflow-x: auto;
        background: white;
        border: 1px solid #e5e7eb;
        border-radius: 16px;
    }

    table {
        width: 100%;
        border-collapse: collapse;
        min-width: 900px;
    }

    th,
    td {
        padding: 1rem;
        text-align: left;
        border-bottom: 1px solid #e5e7eb;
        vertical-align: middle;
    }

    th {
        color: #374151;
        font-weight: 700;
        background: #f9fafb;
    }

    td img {
        width: 80px;
        height: 80px;
        object-fit: cover;
        border-radius: 12px;
        border: 1px solid #e5e7eb;
    }

    .actions {
        display: flex;
        gap: 0.75rem;
    }

    .btn-primary,
    .btn-secondary,
    .btn-danger {
        border: none;
        padding: 0.75rem 1rem;
        border-radius: 10px;
        cursor: pointer;
        font-weight: 600;
        transition: transform 0.15s ease, background 0.15s ease;
    }

    .btn-primary {
        background: #c9a227;
        color: white;
    }

    .btn-secondary {
        background: #f3f4f6;
        color: #111827;
    }

    .btn-danger {
        background: #ef4444;
        color: white;
    }

    .btn-primary:hover,
    .btn-secondary:hover,
    .btn-danger:hover {
        transform: translateY(-1px);
        opacity: 0.95;
    }
</style>
