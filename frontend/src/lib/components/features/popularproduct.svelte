<script lang="ts">
    import { onMount } from "svelte";
    import { fetchJewelry } from "$lib/services/jewelryService";
    import type { JewelryItem } from "$lib/types/jewelry";
    import necklace from "$lib/img/necklace.png";
    import { addToCart } from "$lib/services/cartService";
    import { auth } from "$lib/stores/authStore";
    import { goto } from "$app/navigation";

    let products: JewelryItem[] = [];
    let loading = true;
    let error = "";

    onMount(async () => {
        try {
            products = await fetchJewelry();
            loading = false;
        } catch (e: any) {
            error = e.message;
            loading = false;
        }
    });

    async function handleAddToCart(productId: number) {
        if (!$auth) {
            goto('/login');
            return;
        }
        try {
            await addToCart(productId, 1);
            alert('Item added to cart!');
        } catch (e: any) {
            alert('Failed to add to cart: ' + e.message);
        }
    }

    let items = [
        {
            id: 1,
            text: "necklace",
            img: necklace,
        },
    ];
</script>

<section class="bg-[#E5DED5]">
    <section class="bg-[#E5DED5] px-10 py-5">
        <h3 class="capitalize text-[20px]">this week</h3>
        <h2 class="capitalize text-[28px]">popular product</h2>
    </section>
    <section class="">
        <div class="grid grid-cols-4 gap-10 px-10 flex justify-center items-center">
            {#if loading}
                <p>Loading products...</p>
            {:else if error}
                <p class="text-red-500">{error}</p>
            {:else}
                {#each products as product (product.id)}
                    <div class=" ">
                        <img
                            class="h-64 w-full object-cover rounded-t-lg bg-gray-200"
                            src={product.imageUrl || 'https://via.placeholder.com/400x300?text=Jewelry'}
                            alt={product.name}
                            on:error={(e) => e.currentTarget.src = 'https://via.placeholder.com/400x300?text=Image+Not+Found'}
                        />
                        <div class="bg-[#DBCDBD] py-2 px-4 w-64">
                            <span
                                class="flex justify-center text-[16px] w-full">
                                {product.name}
                            </span>
                            <span class="text-[14px] flex justify-center">
                                {product.description}
                            </span>
                            <div class="flex flex-col items-center gap-2 mt-2">
                                <span class="font-bold text-[#c9a227]">Rs {product.price}</span>
                                <button 
                                    class="bg-white border border-[#c9a227] text-[#c9a227] hover:bg-[#c9a227] hover:text-white px-4 py-1 text-sm transition-all duration-300"
                                    on:click={() => handleAddToCart(product.id)}>
                                    Add to Cart
                                </button>
                            </div>
                        </div>
                    </div>
                {/each}
            {/if}
        </div>
    </section>
    <section class="mt-10 px-10 relative">
        <div class="grid grid-cols-4">
            {#each items as item}
                <div>
                    <img
                        class="h-90 w-60 object-cover"
                        src={necklace}
                        alt="necklace"/>
                </div>
                <div
                    class="absolute top-30 text-black text-[36px] uppercase left-17">
                    {item.text}
                </div>
            {/each}
        </div>
    </section>
   
</section>
