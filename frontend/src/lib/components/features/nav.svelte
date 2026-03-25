<script lang="ts">
	import { auth, logout } from '$lib/stores/authStore';
	export let links = [
		{ name: "Home", url: "/" },
		{ name: "About", url: "/about" },
		{ name: "Contact", url: "/contact" },
	];
</script>

<nav class="text-black">
	<ul class="flex">
		{#each links as link}
			<li>
				<a href={link.url}>{link.name}</a>
			</li>
		{/each}
		
		{#if $auth}
			<li>
				<span>Hello, {$auth.username}</span>
			</li>
			<li>
				<button onclick={logout} class="logout-btn">Logout</button>
			</li>
		{:else}
			<li>
				<a href="/login">Login</a>
			</li>
			<li>
				<a href="/register">Register</a>
			</li>
		{/if}
	</ul>
</nav>

<style>
	.logout-btn {
		background: none;
		border: none;
		color: red;
		cursor: pointer;
		font: inherit;
		padding: 0;
	}
</style>

<!-- <style>
	nav {
		background: #222;
		padding: 1rem;
	}

	ul {
		list-style: none;
		display: flex;
		gap: 20px;
		margin: 0;
		padding: 0;
	}

	a {
		color: white;
		text-decoration: none;
		font-weight: bold;
	}

	a:hover {
		color: #00ffcc;
	}
</style> -->
