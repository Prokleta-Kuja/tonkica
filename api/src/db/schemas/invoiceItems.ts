import { integer, numeric, pgTable, serial, text } from "drizzle-orm/pg-core";
import { invoices } from "./invoices";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";

export const invoiceItems = pgTable("invoice_items", {
  id: serial("id").primaryKey(),
  invoiceId: integer("invoice_id")
    .references(() => invoices.id)
    .notNull(),
  title: text("title").notNull(),
  quantity: numeric("quantity", { precision: 15, scale: 6 }).notNull(),
  price: numeric("price", { precision: 15, scale: 6 }).notNull(),
  total: numeric("total", { precision: 15, scale: 6 }).notNull(),
});

export const insertInvoiceItemSchema = createInsertSchema(invoiceItems);
export const selectInvoiceItemSchema = createSelectSchema(invoiceItems);
