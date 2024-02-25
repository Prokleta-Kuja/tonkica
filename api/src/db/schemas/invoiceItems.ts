import { integer, pgTable, serial, text } from "drizzle-orm/pg-core";
import { invoices } from "./invoices";
import { createInsertSchema } from "drizzle-zod";
import { numericNumber } from "@db/numericNumber";
import { z } from "zod";

export const invoiceItems = pgTable("invoice_items", {
  id: serial("id").primaryKey(),
  invoiceId: integer("invoice_id")
    .references(() => invoices.id)
    .notNull(),
  title: text("title").notNull(),
  quantity: numericNumber("quantity").notNull(),
  price: numericNumber("price").notNull(),
  total: numericNumber("total").notNull(),
});

export const insertInvoiceItemSchema = createInsertSchema(invoiceItems, {
  quantity: z.number(),
  price: z.number(),
  total: z.number(),
});
