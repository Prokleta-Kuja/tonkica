import { drizzle } from "drizzle-orm/postgres-js";
import { migrate } from "drizzle-orm/postgres-js/migrator";
import postgres from "postgres";
import { env } from "../utils";
import * as schema from "./schemas";

export let db: ReturnType<typeof drizzle<typeof schema>>;

export const initDb = async () => {
  if (!env.IS_DEVELOPMENT) {
    const migrationClient = postgres(env.DATABASE_URL, { max: 1 });
    await migrate(drizzle(migrationClient), {
      migrationsFolder: "./src/db/migrations",
    })
      .then(() => {
        console.info("DB migration complete");
      })
      .catch((error) => {
        const errorStr = `Failed to migrate database ${String(error)}`;
        console.error(errorStr);
        throw new Error(errorStr);
      });
    await migrationClient.end();
  }

  const queryClient = postgres(env.DATABASE_URL);
  db = drizzle(queryClient, { schema });
};