<script lang="ts">
	import favicon from "$lib/assets/favicon.svg";
	import Nav from "$lib/component/nav.svelte";
	 import '../app.css';

	let { children } = $props();

	const links = [
		{ name: "Home", url: "/" },
		{ name: "About", url: "/about" },
		{ name: "Contact", url: "/contact" },
	];
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

<Nav {links} />

<main style="padding: 0px; " class="">
	{@render children()}
</main>
