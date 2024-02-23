import { bigint, integer, pgTable, serial, text } from "drizzle-orm/pg-core";
import { invoices } from "./invoices";

export const invoiceItems = pgTable("invoice_items", {
  id: serial("id").primaryKey(),
  invoiceId: integer("invoice_id")
    .references(() => invoices.id)
    .notNull(),
  title: text("title").notNull(),
  quantity: bigint("quantity", { mode: "bigint" }).notNull(),
  price: bigint("price", { mode: "bigint" }).notNull(),
  total: bigint("total", { mode: "bigint" }).notNull(),
});
