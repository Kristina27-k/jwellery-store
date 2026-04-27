<script lang="ts">
    import { createJewelry } from "$lib/services/jewelryService";
    import { goto } from "$app/navigation";
    import { onMount } from "svelte";
    import { PUBLIC_API_BASE_URL } from '$env/static/public';

    let name = "";
    let description = "";
    let price = 0;
    let categoryId = 1;
    let imageFile: File | null = null;
    let loading = false;
    let error = "";
    let uploadProgress = false;

    async function handleFileChange(event: Event) {
        const target = event.target as HTMLInputElement;
        if (target.files && target.files[0]) {
            imageFile = target.files[0];
        }
    }

    async function uploadImage(): Promise<string> {
        if (!imageFile) return "https://images.unsplash.com/photo-1515562141207-7a1891ce32c3?auto=format&fit=crop&q=80&w=800";

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
        loading = true;
        error = "";
        try {
            uploadProgress = true;
            const uploadedUrl = await uploadImage();
            uploadProgress = false;

            await createJewelry({
                name,
                description,
                price,
                categoryId,
                imageUrl: uploadedUrl
            });
            goto("/admin");
        } catch (e: any) {
            error = e.message;
        } finally {
            loading = false;
            uploadProgress = false;
        }
    }
</script>

<div class="admin-container">
    <div class="form-card">
        <header>
            <h1>Add New Jewelry</h1>
            <p>Fill in the details to add a new item to the store.</p>
        </header>

        <form on:submit|preventDefault={handleSubmit}>
            <div class="input-group">
                <label for="name">Product Name</label>
                <input
                    type="text"
                    id="name"
                    bind:value={name}
                    placeholder="e.g. Diamond Solitaire Ring"
                    required
                />
            </div>

            <div class="input-group">
                <label for="description">Description</label>
                <textarea
                    id="description"
                    bind:value={description}
                    rows="4"
                    placeholder="Describe the item's beauty and materials..."
                    required
                ></textarea>
            </div>

            <div class="row">
                <div class="input-group">
                    <label for="price">Price (Rs)</label>
                    <input
                        type="number"
                        id="price"
                        bind:value={price}
                        step="0.01"
                        min="0"
                        required
                    />
                </div>

                <div class="input-group">
                    <label for="category">Category</label>
                    <select id="category" bind:value={categoryId}>
                        <option value={1}>Rings</option>
                        <option value={2}>Necklaces</option>
                        <option value={3}>Bracelets</option>
                        <option value={4}>Earrings</option>
                    </select>
                </div>
            </div>

            <div class="input-group">
                <label for="image">Product Image</label>
                <input
                    type="file"
                    id="image"
                    accept="image/*"
                    on:change={handleFileChange}
                    class="file-input"
                />
                <small>Upload a high-quality image of the jewelry.</small>
            </div>

            {#if error}
                <div class="error-message">{error}</div>
            {/if}

            <div class="actions">
                <button
                    type="button"
                    class="btn-secondary"
                    on:click={() => goto("/admin")}>Cancel</button
                >
                <button type="submit" class="btn-primary" disabled={loading}>
                    {loading ? (uploadProgress ? "Uploading Image..." : "Creating...") : "Create Item"}
                </button>
            </div>
        </form>
    </div>
</div>

<style>
    .admin-container {
        min-height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f8f9fa;
        padding: 2rem;
    }

    .form-card {
        background: white;
        width: 100%;
        max-width: 600px;
        padding: 3rem;
        border-radius: 16px;
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.05);
    }

    header {
        margin-bottom: 2.5rem;
        text-align: center;
    }

    h1 {
        font-size: 2rem;
        color: #1a1a1a;
        margin-bottom: 0.5rem;
    }

    header p {
        color: #666;
    }

    .input-group {
        margin-bottom: 1.5rem;
    }

    .row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1.5rem;
    }

    label {
        display: block;
        font-weight: 600;
        margin-bottom: 0.5rem;
        color: #333;
        font-size: 0.9rem;
    }

    input,
    textarea,
    select {
        width: 100%;
        padding: 0.8rem;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        font-size: 1rem;
        transition: all 0.2s;
    }

    input:focus,
    textarea:focus,
    select:focus {
        outline: none;
        border-color: #c9a227;
        box-shadow: 0 0 0 3px rgba(201, 162, 39, 0.1);
    }

    small {
        display: block;
        margin-top: 0.4rem;
        color: #999;
        font-size: 0.8rem;
    }

    .error-message {
        background: #fff5f5;
        color: #e53e3e;
        padding: 1rem;
        border-radius: 8px;
        margin-bottom: 1.5rem;
        font-size: 0.9rem;
        border-left: 4px solid #e53e3e;
    }

    .actions {
        display: flex;
        gap: 1rem;
        margin-top: 2rem;
    }

    button {
        flex: 1;
        padding: 1rem;
        border-radius: 8px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
    }

    .btn-primary {
        background: #c9a227;
        color: white;
        border: none;
    }

    .btn-primary:hover:not(:disabled) {
        background: #b08d20;
        transform: translateY(-2px);
    }

    .btn-secondary {
        background: white;
        color: #666;
        border: 1px solid #e0e0e0;
    }

    .btn-secondary:hover {
        background: #f5f5f5;
    }

    button:disabled {
        opacity: 0.7;
        cursor: not-allowed;
    }
</style>
