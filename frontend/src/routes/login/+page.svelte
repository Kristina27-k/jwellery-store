<script lang="ts">
    import { login } from '$lib/services/authService';
    import { auth } from '$lib/stores/authStore';
    import { goto } from '$app/navigation';
    

    let email = '';
    let password = '';
    let error = '';
    let loading = false;

    async function handleLogin() {
        error = '';
        loading = true;

        try {
            const user = await login(email, password);
            auth.set(user);
            goto('/');
        } catch (e: any) {
            error = e.message;
        } finally {
            loading = false;
        }
    }
</script>

<div class="auth-wrapper">
    <div class="auth-container">
        <h2>Welcome Back</h2>
        <p class="subtitle">Login to Kristy </p>

        <form on:submit|preventDefault={handleLogin}>
            <div class="form-group">
                <label for="email">Email Address</label>
                <input
                    type="email"
                    id="email"
                    bind:value={email}
                    placeholder="Enter your email"
                    required
                />
            </div>

            <div class="form-group">
                <label for="password">Password</label>
                <input
                    type="password"
                    id="password"
                    bind:value={password}
                    placeholder="Enter your password"
                    required
                />
            </div>

            {#if error}
                <p class="error">{error}</p>
            {/if}

            <button type="submit" disabled={loading}>
                {loading ? 'Logging in...' : 'Login'}
            </button>
        </form>

        <p class="register-link">
            Don't have an account?
            <a href="/register">Create one</a>
        </p>
    </div>
</div>

<style>
    .auth-wrapper {
        min-height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        background: linear-gradient(135deg, #f9f5f0, #f1e6d6);
        font-family: 'Segoe UI', sans-serif;
    }

    .auth-container {
        width: 100%;
        max-width: 420px;
        padding: 2.5rem;
        background: white;
        border-radius: 12px;
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.08);
        text-align: center;
    }

    h2 {
        margin-bottom: 0.3rem;
        font-size: 1.8rem;
        color: #333;
    }

    .subtitle {
        margin-bottom: 2rem;
        color: #888;
        font-size: 0.95rem;
    }

    .form-group {
        text-align: left;
        margin-bottom: 1.4rem;
    }

    label {
        display: block;
        margin-bottom: 0.4rem;
        font-size: 0.9rem;
        color: #555;
    }

    input {
        width: 100%;
        padding: 0.65rem;
        border: 1px solid #ddd;
        border-radius: 6px;
        transition: border 0.2s, box-shadow 0.2s;
        font-size: 0.95rem;
    }

    input:focus {
        outline: none;
        border-color: #c9a227;
        box-shadow: 0 0 0 2px rgba(201, 162, 39, 0.15);
    }

    button {
        width: 100%;
        padding: 0.8rem;
        margin-top: 0.5rem;
        background: linear-gradient(90deg, #c9a227, #e6c35c);
        color: white;
        border: none;
        border-radius: 6px;
        font-weight: 600;
        letter-spacing: 0.5px;
        cursor: pointer;
        transition: transform 0.15s ease, box-shadow 0.15s ease;
    }

    button:hover:not(:disabled) {
        transform: translateY(-1px);
        box-shadow: 0 6px 14px rgba(0, 0, 0, 0.12);
    }

    button:disabled {
        background: #ccc;
        cursor: not-allowed;
    }

    .error {
        color: #d9534f;
        font-size: 0.85rem;
        margin-bottom: 0.5rem;
        text-align: left;
    }

    .register-link {
        margin-top: 1.5rem;
        font-size: 0.9rem;
        color: #666;
    }

    .register-link a {
        color: #c9a227;
        text-decoration: none;
        font-weight: 600;
    }

    .register-link a:hover {
        text-decoration: underline;
    }
</style>
