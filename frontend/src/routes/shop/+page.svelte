<script lang="ts">
	import { cart } from "$lib/stores/cartStores";

	const product = {
		id: 1,
		name: "Sample Product",
		price: 100
	};
</script>

<button
	on:click={() => cart.update(items => [...items, product])}
	class="bg-pink-500 text-white px-4 py-2 rounded-lg"
>
	Add to Cart
</button>