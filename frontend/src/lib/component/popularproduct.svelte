<script>
    import opacuityimg from "$lib/img/opacity.png";
    import heeroing from "$lib/img/bg.png";
    import earring from "$lib/img/earring.png";
    import Nav from "$lib/component/nav.svelte";
    import necklace from "$lib/img/necklace.png";
    // import "../app.css";
    let products = [
        {
            id: 1,
            name: "Pearls with Gold",
            description: "14 k Gold Earrings with Pearls",
            price: 6000,
            img: earring,
        },

        {
            id: 2,
            name: "Pearls with Gold",
            description: "14 k Gold Earrings with Pearls",
            price: 6000,
            img: earring,
        },
        {
            id: 3,
            name: "Pearls with Gold",
            description: "14 k Gold Earrings with Pearls",
            price: 6000,
            img: earring,
        },
        {
            id: 4,
            name: "Pearls with Gold",
            description: "14 k Gold Earrings with Pearls",
            price: 6000,
            img: earring,
        },
    ];
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
            {#each products as product (product.id)}
                <div class=" ">
                    <img
                        class="h-50 w-50 object-cover"
                        src={product.img}
                        alt="earring"
                    />
                    <div class="bg-[#DBCDBD] py-2 px-4 w-64">
                        <span class="flex justify-center text-[16px] w-full">
                            {product.name}
                        </span>
                        <span class="text-[14px] flex justify-center">
                            {product.description}
                        </span>
                        <div class="flex justify-center text-[14px]">
                            <button class="bg-white px-5 py-0.5 mt-1">
                                {product.price}</button
                            >
                        </div>
                    </div>
                </div>
            {/each}
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
                <div class="absolute top-30 text-black text-[36px] uppercase left-17">
                    {item.text}
                </div>
            {/each}
        </div>
    </section>
</section>
