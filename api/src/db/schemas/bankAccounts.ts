import { index, integer, pgTable, serial, text } from "drizzle-orm/pg-core";
import { createInsertSchema, createSelectSchema } from "drizzle-zod";
import { issuers } from "./issuers";
import { relations } from "drizzle-orm";

export const bankAccounts = pgTable(
  "bank_accounts",
  {
    id: serial("id").primaryKey(),
    name: text("name").notNull(),
    info: text("info").notNull(),
    currency: text("currency").notNull(),
    issuerId: integer("issuer_id")
      .references(() => issuers.id)
      .notNull(),
  },
  (t) => ({ issuerIdIdx: index("bank_accounts_issuer_id_idx").on(t.issuerId) })
);

export const insertBankAccountSchema = createInsertSchema(bankAccounts);
export const selectBankAccountSchema = createSelectSchema(bankAccounts);
export const bankAccountsRelations = relations(bankAccounts, ({ one }) => ({
  issuer: one(issuers, {
    fields: [bankAccounts.issuerId],
    references: [issuers.id],
  }),
}));
