import { numeric, pgTable, serial, smallint, text } from "drizzle-orm/pg-core";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";

export const clients = pgTable("clients", {
  id: serial("id").primaryKey(),
  name: text("name").notNull(),
  info: text("info").notNull(),
  currency: text("currency").notNull(),
  rate: numeric("rate", { precision: 15, scale: 6 }).notNull(),
  daysDue: smallint("days_due").notNull(),
  tz: text("tz").notNull().default("America/New_York"),
  locale: text("locale").notNull().default("en-US"),
});

export const insertClientSchema = createInsertSchema(clients);
export const selectClientSchema = createSelectSchema(clients);
