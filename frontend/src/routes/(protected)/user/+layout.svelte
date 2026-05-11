<script lang="ts">
    import { page } from '$app/stores';
    import { logout } from '$lib/services/authService';

    async function handleLogout() {
        await logout();
        window.location.href = '/login';
    }

    const links = [
        { href: '/user', label: 'My Account', icon: 'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z' },
        { href: '/user/orders', label: 'My Orders', icon: 'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4' },
        { href: '/cart', label: 'Cart', icon: 'M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z' },
        { href: '/', label: 'Back to Shop', icon: 'M10 19l-7-7m0 0l7-7m-7 7h18' }
    ];
</script>

<div class="flex h-screen bg-gray-50 font-sans">
    <!-- Sidebar -->
    <aside class="w-64 bg-white border-r border-gray-200 flex flex-col hidden md:flex">
        <div class="p-6 border-b border-gray-200">
            <h2 class="text-2xl font-bold text-gray-800 tracking-tight">User<span class="text-blue-500">Dashboard</span></h2>
        </div>
        <nav class="flex-1 p-4 space-y-2 overflow-y-auto">
            {#each links as link}
                <a
                    href={link.href}
                    class="flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 font-medium { $page.url.pathname === link.href ? 'bg-blue-500 text-white shadow-md shadow-blue-200' : 'text-gray-600 hover:bg-blue-50 hover:text-blue-600' }"
                >
                    <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d={link.icon} />
                    </svg>
                    {link.label}
                </a>
            {/each}
        </nav>
        <div class="p-4 border-t border-gray-200">
            <button
                on:click={handleLogout}
                class="flex items-center gap-3 px-4 py-3 w-full text-left text-red-600 hover:bg-red-50 rounded-xl transition-colors font-medium"
            >
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                </svg>
                Logout
            </button>
        </div>
    </aside>

    <!-- Main Content -->
    <main class="flex-1 overflow-y-auto">
        <!-- Mobile Header -->
        <header class="md:hidden bg-white border-b border-gray-200 p-4 flex justify-between items-center">
            <h2 class="text-xl font-bold text-gray-800">User<span class="text-blue-500">Dashboard</span></h2>
            <button class="text-gray-500 hover:text-gray-700">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                </svg>
            </button>
        </header>

        <slot />
    </main>
</div>
