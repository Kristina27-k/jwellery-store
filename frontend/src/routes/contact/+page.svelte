<script lang="ts">
	let name = "";
	let email = "";
	let message = "";

	function handleSubmit() {
		alert(`Thanks ${name}! We received your message.`);
		name = "";
		email = "";
		message = "";
	}
</script>

<div class="min-h-screen flex items-center justify-center bg-gray-50 px-4">
	<div class="bg-white shadow-lg rounded-2xl p-8 w-full max-w-xl">
		<h1 class="text-3xl font-bold text-center mb-6 text-pink-500">
			Contact Kristy Store
		</h1>

		<form on:submit|preventDefault={handleSubmit} class="space-y-4">
			<!-- Name -->
			<input
				type="text"
				placeholder="Your Name"
				bind:value={name}
				class="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-pink-400"
				required
			/>

			<!-- Email -->
			<input
				type="email"
				placeholder="Your Email"
				bind:value={email}
				class="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-pink-400"
				required
			/>

			<!-- Message -->
			<textarea
				rows="5"
				placeholder="Your Message"
				bind:value={message}
				class="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-pink-400"
				required
			></textarea>

			<!-- Button -->
			<button
				type="submit"
				class="w-full bg-pink-500 hover:bg-pink-600 text-white py-2 rounded-lg transition"
			>
				Send Message
			</button>
		</form>
	</div>
</div>