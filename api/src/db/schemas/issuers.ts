import { relations } from "drizzle-orm";
import { pgTable, serial, text } from "drizzle-orm/pg-core";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";
import { bankAccounts } from "./bankAccounts";
import { invoices } from "./invoices";

export const issuers = pgTable("issuers", {
  id: serial("id").primaryKey(),
  name: text("name").notNull(),
  info: text("info").notNull(),
  currency: text("currency").notNull(),
  tz: text("tz").notNull().default("Europe/Zagreb"),
  locale: text("locale").notNull().default("hr-HR"),
  invoicedBy: text("invoiced_by").notNull(),
  logo: text("logo"),
});

export const insertIssuerSchema = createInsertSchema(issuers);
export const selectIssuerSchema = createSelectSchema(issuers);
export const issuersRelations = relations(issuers, ({ many }) => ({
  bankAccounts: many(bankAccounts),
  invoices: many(invoices),
}));
