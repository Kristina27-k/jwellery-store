<script lang="ts">
    import { page } from '$app/stores';
    import { logout } from '$lib/stores/authStore';

    function handleLogout() {
        logout();
        window.location.href = '/login';
    }

    const links = [
        { href: '/admin', label: 'Dashboard', icon: 'M4 6h16M4 12h16M4 18h16' },
        { href: '/admin/items', label: 'Jewelry Items', icon: 'M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4' },
        { href: '/admin/categories', label: 'Categories', icon: 'M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z' },
        { href: '/admin/orders', label: 'Orders', icon: 'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4' },
        { href: '/', label: 'Back to Shop', icon: 'M10 19l-7-7m0 0l7-7m-7 7h18' }
    ];
</script>

<div class="flex h-screen bg-gray-50 font-sans">
    <!-- Sidebar -->
    <aside class="w-64 bg-white border-r border-gray-200 flex flex-col hidden md:flex">
        <div class="p-6 border-b border-gray-200">
            <h2 class="text-2xl font-bold text-gray-800 tracking-tight">Admin<span class="text-pink-500">Panel</span></h2>
        </div>
        <nav class="flex-1 p-4 space-y-2 overflow-y-auto">
            {#each links as link}
                <a
                    href={link.href}
                    class="flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 font-medium { $page.url.pathname === link.href || ($page.url.pathname.startsWith(link.href) && link.href !== '/' && link.href !== '/admin') ? 'bg-pink-500 text-white shadow-md shadow-pink-200' : 'text-gray-600 hover:bg-pink-50 hover:text-pink-600' }"
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
            <h2 class="text-xl font-bold text-gray-800">Admin<span class="text-pink-500">Panel</span></h2>
            <button class="text-gray-500 hover:text-gray-700">
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                </svg>
            </button>
        </header>

        <slot />
    </main>
</div>
