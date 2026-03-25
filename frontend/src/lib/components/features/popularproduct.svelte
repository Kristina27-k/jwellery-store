<script lang="ts">
    import { onMount } from "svelte";
    import { fetchJewelry } from "$lib/services/jewelryService";
    import type { JewelryItem } from "$lib/types/jewelry";
    import necklace from "$lib/img/necklace.png";

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
        <div
            class="grid grid-cols-4 gap-10 px-10 flex justify-center items-center"
        >
            {#if loading}
                <p>Loading products...</p>
            {:else if error}
                <p class="text-red-500">{error}</p>
            {:else}
                {#each products as product (product.id)}
                    <div class=" ">
                        <img
                            class="h-50 w-50 object-cover"
                            src={product.imageUrl}
                            alt={product.name}
                        />
                        <div class="bg-[#DBCDBD] py-2 px-4 w-64">
                            <span
                                class="flex justify-center text-[16px] w-full"
                            >
                                {product.name}
                            </span>
                            <span class="text-[14px] flex justify-center">
                                {product.description}
                            </span>
                            <div class="flex justify-center text-[14px]">
                                <button class="bg-white px-5 py-0.5 mt-1">
                                    Rs{product.price}</button
                                >
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
                        alt="necklace"
                    />
                </div>
                <div
                    class="absolute top-30 text-black text-[36px] uppercase left-17"
                >
                    {item.text}
                </div>
            {/each}
        </div>
    </section>
</section>
