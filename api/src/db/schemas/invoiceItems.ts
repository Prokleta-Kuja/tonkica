import { index, integer, pgTable, serial, text } from "drizzle-orm/pg-core";
import { invoices } from "./invoices";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";
import { numericNumber } from "@db/numericNumber";
import { z } from "zod";
import { relations } from "drizzle-orm";

export const invoiceItems = pgTable(
  "invoice_items",
  {
    id: serial("id").primaryKey(),
    invoiceId: integer("invoice_id")
      .references(() => invoices.id)
      .notNull(),
    title: text("title").notNull(),
    quantity: numericNumber("quantity").notNull(),
    price: numericNumber("price").notNull(),
    total: numericNumber("total").notNull(),
  },
  (t) => ({
    invoiceIdIdx: index("invoice_items_invoice_id_idx").on(t.invoiceId),
  })
);

export const insertInvoiceItemSchema = createInsertSchema(invoiceItems, {
  quantity: z.number(),
  price: z.number(),
  total: z.number(),
});
export const selectInvoiceItemSchema = createSelectSchema(invoiceItems);
export const invoiceItemsRelations = relations(invoiceItems, ({ one }) => ({
  invoice: one(invoices, {
    fields: [invoiceItems.invoiceId],
    references: [invoices.id],
  }),
}));
