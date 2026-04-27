<script lang="ts">
	let avatar: string | null = null;
	let fileinput: HTMLInputElement;

	const onFileSelected = (e: Event) => {
		const target = e.target as HTMLInputElement;

		if (!target.files || target.files.length === 0) return;

		const image: File = target.files[0];

		// Optional validation
		if (!image.type.startsWith("image/")) {
			alert("Please select a valid image file");
			return;
		}

		const reader = new FileReader();

		reader.readAsDataURL(image);

		reader.onload = (event: ProgressEvent<FileReader>) => {
			avatar = event.target?.result as string;
		};
	};

	const triggerFileInput = () => {
		fileinput.click();
	};
</script>

<div id="app">
	<h1>Upload Image</h1>

	{#if avatar}
		<img class="avatar" src={avatar} alt="avatar preview" />
	{:else}
		<img
			class="avatar"
			src="https://cdn4.iconfinder.com/data/icons/small-n-flat/24/user-alt-512.png"
			alt="default avatar"
		/>
	{/if}

	<img
		class="upload"
		src="https://static.thenounproject.com/png/625182-200.png"
		alt="upload icon"
		on:click={triggerFileInput}
	/>

	<div class="chan" on:click={triggerFileInput}>
		Choose Image
	</div>

	<input
		type="file"
		accept=".jpg, .jpeg, .png"
		on:change={onFileSelected}
		bind:this={fileinput}
		hidden
	/>
</div>

<style>
	#app {
		display: flex;
		align-items: center;
		justify-content: center;
		flex-direction: column;
		gap: 10px;
		font-family: Arial, sans-serif;
	}

	h1 {
		margin-bottom: 10px;
	}

	.avatar {
		height: 200px;
		width: 200px;
		object-fit: cover;
		border-radius: 50%;
		border: 2px solid #ccc;
	}

	.upload {
		height: 50px;
		width: 50px;
		cursor: pointer;
		transition: transform 0.2s;
	}

	.upload:hover {
		transform: scale(1.1);
	}

	.chan {
		padding: 8px 16px;
		background-color: #007bff;
		color: white;
		border-radius: 6px;
		cursor: pointer;
		transition: background-color 0.2s;
	}

	.chan:hover {
		background-color: #0056b3;
	}
</style>