import { integer, pgTable, serial, text, timestamp } from "drizzle-orm/pg-core";
import { clients } from "./clients";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";

export const tasks = pgTable("tasks", {
  id: serial("id").primaryKey(),
  clientId: integer("client_id")
    .references(() => clients.id)
    .notNull(),
  title: text("title").notNull(),
  created: timestamp("created"),
});

export const insertTaskSchema = createInsertSchema(tasks);
export const selectTaskSchema = createSelectSchema(tasks)