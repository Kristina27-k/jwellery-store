import { redirect, type Handle } from '@sveltejs/kit';

export const handle: Handle = async ({ event, resolve }) => {
    const userCookie = event.cookies.get('user');
    const user = userCookie ? JSON.parse(decodeURIComponent(userCookie)) : null;

    const path = event.url.pathname;

    // Protection logic
    // Admin routes
    if (path.startsWith('/admin')) {
        if (!user) {
            throw redirect(303, '/login');
        }
        if (user.role !== 'Admin') {
            throw redirect(303, '/user'); // Redirect non-admins to their user page
        }
    }

    // General user routes
    if (path.startsWith('/user')) {
        if (!user) {
            throw redirect(303, '/login');
        }
    }

    const response = await resolve(event);
    return response;
};
