import { z } from "zod";
import "dotenv/config";

const envSchema = z.object({
  IS_DEVELOPMENT: z
    .string()
    .toLowerCase()
    .transform((x) => x === "true" || x === "1")
    .pipe(z.boolean().default(false)),
  DATABASE_URL: z
    .string()
    .default(
      "postgres://postgres:postgres@db:5432/tonkica?application_name=tonkica"
    ),
});

export const env = envSchema.parse(process.env);