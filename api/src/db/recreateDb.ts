import { env } from "@utils/env";
import postgres from "postgres";
import { ensureDevData } from "./dev";
import { sleep } from "@utils/index";

const recreateDb = async () => {
  if (!env.IS_DEVELOPMENT) {
    console.error("NOT dev environment, stopping");
    return;
  }

  const sql = postgres("postgres://postgres:postgres@db:5432/postgres");
  const db = postgres(env.DATABASE_URL);

  const databases = await sql`SELECT datname FROM pg_database
  WHERE datistemplate = false;`;

  let found = false;
  for (let i = 0; i < databases.length; i++) {
    const row = databases[i];
    if (row.datname === db.options.database) {
      found = true;
      console.log("Deleting database", db.options.database);
      await sql`DROP DATABASE ${sql(db.options.database)} WITH (FORCE);`;
    }
  }
  if (!found) console.log("No dev db to delete");

  console.log("Creating database", db.options.database);
  await sql`CREATE DATABASE ${sql(db.options.database)}
    WITH
    OWNER = ${sql(db.options.user)}
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = 'libc'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;`;

  console.log("Database created");

  process.exit();
};

recreateDb();
