<script lang="ts">
    import { auth } from '$lib/stores/authStore';
</script>

<div class="container mx-auto p-8">
    <h1 class="text-3xl font-bold mb-4">My Account</h1>
    <p class="mb-4">Hello, {$auth?.username}! This is your personal area.</p>
    
    <div class="bg-white p-6 rounded-lg shadow-md max-w-lg">
        <h2 class="text-xl font-semibold mb-4">Profile Information</h2>
        <p><strong>Username:</strong> {$auth?.username}</p>
        <p><strong>Email:</strong> {$auth?.email}</p>
        <p><strong>Role:</strong> {$auth?.role}</p>
    </div>
</div>
