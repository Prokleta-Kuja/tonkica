import { pgTable, serial, text } from "drizzle-orm/pg-core";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";

export const bankAccounts = pgTable("bank_accounts", {
  id: serial("id").primaryKey(),
  name: text("name").notNull(),
  info: text("info").notNull(),
  currency: text("currency").notNull(),
});

export const insertBankAccountSchema = createInsertSchema(bankAccounts);
export const selectBankAccountSchema = createSelectSchema(bankAccounts);
