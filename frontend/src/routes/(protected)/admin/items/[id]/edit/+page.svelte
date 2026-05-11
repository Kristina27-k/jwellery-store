<script lang="ts">
    import { onMount } from "svelte";
    import { page } from "$app/stores";
    import { goto } from "$app/navigation";
    import { PUBLIC_API_BASE_URL } from '$env/static/public';
    import type { JewelryItem } from "$lib/types/jewelry";
    import { fetchJewelryById, updateJewelry } from "$lib/services/jewelryService";

    let itemId = 0;
    let item: JewelryItem | null = null;
    let name = "";
    let description = "";
    let price = 0;
    let categoryId = 1;
    let imageUrl = "";
    let imageFile: File | null = null;
    let loading = false;
    let saving = false;
    let error = "";

    $: itemId = Number($page.params.id);

    async function loadItem() {
        loading = true;
        error = "";
        try {
            item = await fetchJewelryById(itemId);
            name = item.name;
            description = item.description;
            price = item.price;
            categoryId = item.categoryId;
            imageUrl = item.imageUrl;
        } catch (e: any) {
            error = e.message || "Unable to load item.";
        } finally {
            loading = false;
        }
    }

    async function handleFileChange(event: Event) {
        const target = event.target as HTMLInputElement;
        if (target.files && target.files[0]) {
            imageFile = target.files[0];
        }
    }

    async function uploadImage(): Promise<string> {
        if (!imageFile) return imageUrl;

        const formData = new FormData();
        formData.append("file", imageFile);

        const response = await fetch(`${PUBLIC_API_BASE_URL}/Upload`, {
            method: "POST",
            body: formData
        });

        if (!response.ok) throw new Error("Failed to upload image");
        const data = await response.json();
        return data.url;
    }

    async function handleSubmit() {
        saving = true;
        error = "";

        try {
            const finalImageUrl = await uploadImage();
            await updateJewelry({
                id: itemId,
                name,
                description,
                price,
                categoryId,
                imageUrl: finalImageUrl
            });
            goto("/admin/items");
        } catch (e: any) {
            error = e.message || "Unable to save changes.";
        } finally {
            saving = false;
        }
    }

    onMount(() => {
        loadItem();
    });
</script>

<div class="admin-container">
    <div class="form-card">
        <header>
            <h1>Edit Jewelry Item</h1>
            <p>Update details for the selected product.</p>
        </header>

        {#if loading}
            <div class="status-card">Loading item details...</div>
        {:else if error}
            <div class="status-card error">{error}</div>
        {:else}
            <form on:submit|preventDefault={handleSubmit}>
                <div class="row">
                    <div class="input-group">
                        <label for="name">Product Name</label>
                        <input id="name" type="text" bind:value={name} required />
                    </div>
                    <div class="input-group">
                        <label for="price">Price (Rs)</label>
                        <input id="price" type="number" bind:value={price} step="0.01" min="0" required />
                    </div>
                </div>

                <div class="input-group">
                    <label for="description">Description</label>
                    <textarea id="description" rows="4" bind:value={description} required></textarea>
                </div>

                <div class="row">
                    <div class="input-group">
                        <label for="category">Category</label>
                        <select id="category" bind:value={categoryId}>
                            <option value={1}>Rings</option>
                            <option value={2}>Necklaces</option>
                            <option value={3}>Bracelets</option>
                            <option value={4}>Earrings</option>
                        </select>
                    </div>
                    <div class="input-group">
                        <label for="image">Product Image</label>
                        <input id="image" type="file" accept="image/*" on:change={handleFileChange} />
                        {#if imageUrl}
                            <img class="preview" src={imageUrl} alt="Product preview" />
                        {/if}
                    </div>
                </div>

                {#if error}
                    <div class="status-card error">{error}</div>
                {/if}

                <div class="actions">
                    <button type="button" class="btn-secondary" on:click={() => goto("/admin/items")}>Cancel</button>
                    <button type="submit" class="btn-primary" disabled={saving}>{saving ? "Saving..." : "Save Changes"}</button>
                </div>
            </form>
        {/if}
    </div>
</div>

<style>
    .admin-container {
        min-height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f8fafc;
        padding: 2rem;
    }

    .form-card {
        max-width: 800px;
        width: 100%;
        background: white;
        padding: 2rem;
        border-radius: 20px;
        box-shadow: 0 20px 40px rgba(15, 23, 42, 0.08);
    }

    header {
        margin-bottom: 1.75rem;
    }

    h1 {
        font-size: 2rem;
        margin-bottom: 0.5rem;
    }

    p {
        color: #6b7280;
        margin: 0;
    }

    .row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1.5rem;
    }

    .input-group {
        margin-bottom: 1.5rem;
    }

    label {
        display: block;
        margin-bottom: 0.5rem;
        color: #374151;
        font-weight: 600;
    }

    input,
    textarea,
    select {
        width: 100%;
        padding: 0.9rem 1rem;
        border-radius: 12px;
        border: 1px solid #d1d5db;
        background: #f9fafb;
        color: #111827;
        font-size: 1rem;
    }

    textarea {
        resize: vertical;
    }

    .preview {
        margin-top: 1rem;
        width: 100%;
        max-height: 240px;
        object-fit: cover;
        border-radius: 14px;
        border: 1px solid #e5e7eb;
    }

    .actions {
        display: flex;
        justify-content: flex-end;
        gap: 1rem;
        margin-top: 1.5rem;
    }

    .btn-primary,
    .btn-secondary {
        border: none;
        cursor: pointer;
        border-radius: 12px;
        padding: 0.95rem 1.25rem;
        font-weight: 700;
    }

    .btn-primary {
        background: #c9a227;
        color: white;
    }

    .btn-secondary {
        background: #f3f4f6;
        color: #111827;
    }

    .status-card {
        padding: 1rem;
        border-radius: 14px;
        border: 1px solid #e5e7eb;
        background: #f8fafc;
        color: #111827;
        margin-bottom: 1.5rem;
    }

    .status-card.error {
        border-color: #fecaca;
        background: #fff1f2;
        color: #991b1b;
    }
</style>
