// place files you want to import through the `$lib` alias in this folder.
import { z } from '';

export const registerSchema = z.object({
  email: z.string().email(),
  password: z.string().min(6)
});
