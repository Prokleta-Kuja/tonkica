import { bigint, pgTable, serial, smallint, text } from "drizzle-orm/pg-core";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";

export const clients = pgTable("clients", {
  id: serial("id").primaryKey(),
  name: text("name").notNull(),
  info: text("info").notNull(),
  currency: text("currency").notNull(),
  rate: bigint("rate", { mode: "bigint" }), //TODO: multiply/divide by 1 000 000
  daysDue: smallint("days_due"),
  tz: text("tz").notNull().default("America/New_York"),
  locale: text("locale").notNull().default("en-US"),
});

export const insertIssuerSchema = createInsertSchema(clients);
export const selectIssuerSchema = createSelectSchema(clients);
