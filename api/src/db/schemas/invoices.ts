import { integer, pgTable, serial, text, timestamp } from "drizzle-orm/pg-core";
import { issuers } from "./issuers";
import { clients } from "./clients";
import { bankAccounts } from "./bankAccounts";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";
import { numericNumber } from "@db/numericNumber";
import { z } from "zod";

export const invoices = pgTable("invoices", {
  id: serial("id").primaryKey(),
  issuerId: integer("issuer_id")
    .references(() => issuers.id)
    .notNull(),
  clientId: integer("client_id")
    .references(() => clients.id)
    .notNull(),
  bankAccountId: integer("bank_account_id")
    .references(() => bankAccounts.id)
    .notNull(),
  sequenceNumber: text("sequence_number").notNull(),
  subject: text("subject").notNull(),
  currency: text("currency").notNull(),
  displayCurrency: text("display_currency").notNull(),
  displayRate: numericNumber("display_rate").notNull(),
  published: timestamp("published").notNull(),
  paid: timestamp("paid"),
  note: text("note"),
});

export const insertInvoiceSchema = createInsertSchema(invoices, {
  displayRate: z.number(),
});
export const selectInvoiceSchema = createSelectSchema(invoices)