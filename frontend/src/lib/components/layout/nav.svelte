<script lang="ts">
	import { auth, logout } from "$lib/stores/authStore";
	import { cart } from "$lib/stores/cartStores";
	

	export let links = [
		{ name: "Home", url: "/" },
		{ name: "Shop", url: "/shop" },
		{ name: "Contact", url: "/contact" }
	];
</script>

<nav class="bg-white shadow-md px-6 py-3 flex justify-between items-center">
	<!-- Logo -->
	<div class="text-2xl font-bold tracking-wide">
		<span class="text-pink-500">Kristy</span>
		<span class="text-gray-800">Store</span>
	</div>

	<!-- Links -->
	<ul class="flex items-center gap-6">
		{#each links as link}
			<li>
				<a
					href={link.url}
					class="text-gray-700 hover:text-pink-500 transition font-medium"
				>
					{link.name}
				</a>
			</li>
		{/each}

		{#if $auth}
			{#if $auth.role === "Admin"}
				<li>
					<a
						href="/admin"
						class="text-gray-700 hover:text-purple-600 font-medium transition"
					>
						Admin
					</a>
				</li>
			{/if}

			<li>
				<a
					href="/user"
					class="text-gray-700 hover:text-pink-500 font-medium transition"
				>
					Account
				</a>
			</li>

			<li>
				<span class="text-sm text-gray-500">
					 <span class="font-semibold text-gray-800">{$auth.username}</span>
				</span>
			</li>

			<li>
				<button
					on:click={logout}
					class="bg-red-500 hover:bg-red-600 text-white px-4 py-1.5 rounded-lg text-sm transition"
				>
					Logout
				</button>
			</li>
		{:else}
			<li>
				<a
					href="/login"
					class="text-gray-700 hover:text-pink-500 font-medium transition"
				>
					Login
				</a>
			</li>

			<li>
				<a
					href="/register"
					class="bg-pink-500 hover:bg-pink-600 text-white px-4 py-1.5 rounded-lg text-sm transition shadow-sm"
				>
					Register
				</a>
			</li>
		{/if}
	</ul>
	<li class="relative">
	<a href="cart" class="text-gray-700 hover:text-pink-500 text-xl">
		🛒
	</a>

	{#if $cart && $cart.length > 0}
		<span class="absolute -top-2 -right-3 bg-pink-500 text-white text-xs px-1.5 rounded-full">
			{$cart.length}
		</span>
	{/if}
</li>
</nav>