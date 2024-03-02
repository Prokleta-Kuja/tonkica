import { integer, pgTable, serial, timestamp } from "drizzle-orm/pg-core";
import { tasks } from "./tasks";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";

export const taskTimes = pgTable("task_times", {
  id: serial("id").primaryKey(),
  taskId: integer("task_id")
    .references(() => tasks.id)
    .notNull(),
  start: timestamp("start"),
  durationMs: integer("duration"), // miliseconds - diff between two dates
});

export const insertTaskTimeSchema = createInsertSchema(taskTimes);
export const selectTaskTimeSchema = createSelectSchema(taskTimes)