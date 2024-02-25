import { numericNumber } from "@db/numericNumber";
import { pgTable, serial, smallint, text } from "drizzle-orm/pg-core";
import { createInsertSchema } from "drizzle-zod";
import { z } from "zod";

export const clients = pgTable("clients", {
  id: serial("id").primaryKey(),
  name: text("name").notNull(),
  info: text("info").notNull(),
  currency: text("currency").notNull(),
  rate: numericNumber("rate").notNull(),
  daysDue: smallint("days_due").notNull(),
  tz: text("tz").notNull().default("America/New_York"),
  locale: text("locale").notNull().default("en-US"),
});

export const insertClientSchema = createInsertSchema(clients, {
  rate: z.number(),
});
