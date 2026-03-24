<script lang="ts">
	export let links = [
		{ name: "Home", url: "/" },
		{ name: "About", url: "/about" },
		{ name: "Contact", url: "/contact" },
	];
</script>

<nav class="text-black ">
	<ul class="flex">
		{#each links as link}
			<li >
				<a href={link.url}>{link.name}</a>
			</li>
		{/each}
	</ul>
</nav>

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
