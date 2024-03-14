import {
  index,
  integer,
  pgTable,
  serial,
  timestamp,
} from "drizzle-orm/pg-core";
import { tasks } from "./tasks";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";
import { relations } from "drizzle-orm";

export const taskTimes = pgTable(
  "task_times",
  {
    id: serial("id").primaryKey(),
    taskId: integer("task_id")
      .references(() => tasks.id)
      .notNull(),
    start: timestamp("start"),
    durationMs: integer("duration"), // miliseconds - diff between two dates
  },
  (t) => ({
    taskIdIdx: index("task_times_task_id_idx").on(t.taskId),
  })
);

export const insertTaskTimeSchema = createInsertSchema(taskTimes);
export const selectTaskTimeSchema = createSelectSchema(taskTimes);
export const taskTimesRelations = relations(taskTimes, ({ one }) => ({
  task: one(tasks, { fields: [taskTimes.taskId], references: [tasks.id] }),
}));
