<script lang="ts">
    import { register } from '$lib/services/authService';
    import { auth } from '$lib/stores/authStore';
    import { goto } from '$app/navigation';
    import { registerSchema } from '$lib/validation/login';

    let username = '';
    let email = '';
    let password = '';
    let error = '';
    let loading = false;

    async function handleRegister() {
        error = '';
        loading = true;

        // Simple validation check before API call
        const validation = registerSchema.safeParse({ email, password });
        if (!validation.success) {
            error = validation.error.issues[0].message;
            loading = false;
            return;
        }

        try {
            const user = await register(username, email, password);
            auth.set(user);
            goto('/');
        } catch (e: any) {
            error = e.message;
        } finally {
            loading = false;
        }
    }
</script>

<div class="auth-container">
    <h2>Register</h2>
    <form on:submit|preventDefault={handleRegister}>
        <div class="form-group">
            <label for="username">Username</label>
            <input type="text" id="username" bind:value={username} required />
        </div>
        <div class="form-group">
            <label for="email">Email</label>
            <input type="email" id="email" bind:value={email} required />
        </div>
        <div class="form-group">
            <label for="password">Password</label>
            <input type="password" id="password" bind:value={password} required />
        </div>
        {#if error}
            <p class="error">{error}</p>
        {/if}
        <button type="submit" disabled={loading}>
            {loading ? 'Registering...' : 'Register'}
        </button>
    </form>
    <p>Already have an account? <a href="/login">Login</a></p>
</div>

<style>
    .auth-container {
        max-width: 400px;
        margin: 2rem auto;
        padding: 2rem;
        border: 1px solid #ddd;
        border-radius: 8px;
    }
    .form-group {
        margin-bottom: 1rem;
    }
    label {
        display: block;
        margin-bottom: 0.5rem;
    }
    input {
        width: 100%;
        padding: 0.5rem;
        border: 1px solid #ccc;
        border-radius: 4px;
    }
    button {
        width: 100%;
        padding: 0.75rem;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }
    button:disabled {
        background-color: #ccc;
    }
    .error {
        color: red;
        margin-bottom: 1rem;
    }
</style>
