<script>

import { fail } from '@sveltejs/kit';


export const actions = {
  default: async ({ request }) => {
    const data = await request.formData();
    const email = data.get('email');

    if (!email) {
      return fail(400, { error: 'Email required' });
    }

    return { success: true };
  }
};
</script>