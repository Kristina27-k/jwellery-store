<script>
    import Hero from "$lib/components/features/hero.svelte";
</script>

<Hero />

<div class="min-h-screen bg-gradient-to-br from-pink-50 via-white to-pink-100 px-6 py-12">

	<!-- HERO SECTION -->
	<div class="max-w-5xl mx-auto text-center">
		<h1 class="text-4xl md:text-5xl font-bold text-gray-800">
			About <span class="text-pink-500">Kristy Store</span>
		</h1>

		<p class="mt-4 text-gray-600 text-lg">
			We create a simple, stylish, and modern shopping experience for everyone.
		</p>
	</div>

	<!-- CONTENT BOX -->
	<div class="max-w-6xl mx-auto mt-12 grid md:grid-cols-2 gap-10 items-center">

		<!-- LEFT TEXT -->
		<div class="bg-white shadow-xl rounded-2xl p-8">
			<h2 class="text-2xl font-bold text-gray-800 mb-4">
				Who We Are
			</h2>

			<p class="text-gray-600 leading-relaxed mb-4">
				Kristy Store is a modern e-commerce platform built with passion for clean design and smooth user experience.
				We focus on simplicity, speed, and usability.
			</p>

			<p class="text-gray-600 leading-relaxed">
				Our goal is to make online shopping easy, beautiful, and accessible for everyone — from casual buyers to daily shoppers.
			</p>
		</div>

		<!-- RIGHT CARD -->
		<div class="grid gap-6">

			<div class="bg-pink-500 text-white rounded-2xl p-6 shadow-lg">
				<h3 class="text-xl font-semibold">🎯 Our Mission</h3>
				<p class="text-pink-100 mt-2">
					To deliver a smooth and modern shopping experience.
				</p>
			</div>

			<div class="bg-white rounded-2xl p-6 shadow-lg">
				<h3 class="text-xl font-semibold text-gray-800">💡 Our Vision</h3>
				<p class="text-gray-600 mt-2">
					To become a simple yet powerful online store loved by users.
				</p>
			</div>

			<div class="bg-white rounded-2xl p-6 shadow-lg">
				<h3 class="text-xl font-semibold text-gray-800">🚀 Why Us?</h3>
				<p class="text-gray-600 mt-2">
					Fast, clean UI, easy cart system, and modern design approach.
				</p>
			</div>

		</div>
	</div>

	<!-- FOOTER SECTION -->
	<div class="text-center mt-16 text-gray-500 text-sm">
		Made with ❤️ by Kristy Store Team
	</div>
</div>