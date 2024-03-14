import {
  index,
  integer,
  pgTable,
  serial,
  text,
  timestamp,
} from "drizzle-orm/pg-core";
import { clients } from "./clients";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";
import { relations } from "drizzle-orm";
import { taskTimes } from "./taskTimes";

export const tasks = pgTable(
  "tasks",
  {
    id: serial("id").primaryKey(),
    clientId: integer("client_id")
      .references(() => clients.id)
      .notNull(),
    title: text("title").notNull(),
    created: timestamp("created"),
  },
  (t) => ({
    clientIdIdx: index("tasks_client_id_idx").on(t.clientId),
  })
);

export const insertTaskSchema = createInsertSchema(tasks);
export const selectTaskSchema = createSelectSchema(tasks);
export const tasksRelations = relations(tasks, ({ many }) => ({
  taskTimes: many(taskTimes),
}));
