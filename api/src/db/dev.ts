import { z } from "zod";
import { db } from ".";
import { bankAccounts, insertBankAccountSchema } from "./schemas/bankAccounts";
import { Currency } from "@utils/currencies";
import { clients, insertClientSchema } from "./schemas/clients";
import { insertIssuerSchema, issuers } from "./schemas/issuers";
import { insertTaskSchema, tasks } from "./schemas/tasks";
import { insertTaskTimeScheme, taskTimes } from "./schemas/taskTimes";
import { insertInvoiceSchema, invoices } from "./schemas/invoices";
import { insertInvoiceItemSchema, invoiceItems } from "./schemas/invoiceItems";

export const ensureDevData = async () => {
  let existingBankAccount = await db.query.bankAccounts.findFirst();
  if (existingBankAccount) {
    console.info("Dev data already exists, skipping...");
    return;
  }

  console.info("Inserting dev data");

  await db.transaction(async (tx) => {
    // Bank accounts
    let bankUS: z.infer<typeof insertBankAccountSchema> = {
      id: 1,
      name: "US Bank Account",
      currency: Currency.USD,
      info: `Account USD
    Routing no: 123
    `,
    };
    bankUS = insertBankAccountSchema.parse(bankUS);

    let bankEU: z.infer<typeof insertBankAccountSchema> = {
      id: 2,
      name: "Belgian Bank Account",
      currency: Currency.EUR,
      info: `Account EUR
    IBAN EU1234
    `,
    };
    bankEU = insertBankAccountSchema.parse(bankEU);

    // Clients
    let clientUS: z.infer<typeof insertClientSchema> = {
      id: 1,
      name: "US Client",
      info: `Street 123
      City 12345
      United States of America`,
      currency: Currency.USD,
      rate: (125).toFixed(6),
      daysDue: 15,
      tz: "America/New_York",
      locale: "en-US",
    };
    clientUS = insertClientSchema.parse(clientUS);

    let clientDE: z.infer<typeof insertClientSchema> = {
      id: 2,
      name: "DE Client",
      info: `Street 123
      City 12345
      Germany`,
      currency: Currency.EUR,
      rate: (95.75).toString(),
      daysDue: 15,
      tz: "Europe/Berlin",
      locale: "de-DE",
    };
    clientDE = insertClientSchema.parse(clientDE);

    // Issuers
    let issuerA: z.infer<typeof insertIssuerSchema> = {
      id: 1,
      name: "Issuer A",
      info: `Street 123
      City 12345
      Croatia`,
      currency: Currency.EUR,
      tz: "Europe/Zagreb",
      locale: "hr-HR",
      invoicedBy: "Person A",
    };
    issuerA = insertIssuerSchema.parse(issuerA);

    let issuerB: z.infer<typeof insertIssuerSchema> = {
      id: 2,
      name: "Issuer B",
      info: `Street 123
      City 12345
      Croatia`,
      currency: Currency.EUR,
      tz: "Europe/Zagreb",
      locale: "hr-HR",
      invoicedBy: "Person B",
    };
    issuerB = insertIssuerSchema.parse(issuerB);

    // Tasks
    let task1: z.infer<typeof insertTaskSchema> = {
      id: 1,
      clientId: 1,
      title: "First task",
      created: new Date(),
    };
    task1 = insertTaskSchema.parse(task1);

    let task2: z.infer<typeof insertTaskSchema> = {
      id: 2,
      clientId: 1,
      title: "Second task",
      created: new Date(),
    };
    task2 = insertTaskSchema.parse(task2);

    // Task times
    let time1: z.infer<typeof insertTaskTimeScheme> = {
      id: 1,
      taskId: 1,
      start: new Date(),
      durationMs: 2 * 60 * 60 * 1000, // 2hrs
    };
    time1 = insertTaskTimeScheme.parse(time1);
    let time2: z.infer<typeof insertTaskTimeScheme> = {
      id: 2,
      taskId: 1,
      start: new Date(),
      durationMs: 4 * 60 * 60 * 1000, // 4hrs
    };
    time2 = insertTaskTimeScheme.parse(time2);

    // Invoices
    let invoice1: z.infer<typeof insertInvoiceSchema> = {
      id: 1,
      issuerId: 1,
      clientId: 1,
      bankAccountId: 1,
      sequenceNumber: "1-1-1",
      subject: "Services rendered in January",
      currency: Currency.EUR,
      displayCurrency: Currency.USD,
      displayRate: (1.0834).toString(),
      published: new Date(),
      note: "Exempt from VAT pursuant to Article 17, paragraph 1 of the Croatian VAT Act.",
    };
    invoice1 = insertInvoiceSchema.parse(invoice1);

    // Invoice items
    let iitem1: z.infer<typeof insertInvoiceItemSchema> = {
      id: 1,
      invoiceId: 1,
      title: "First task",
      quantity: (6).toString(),
      price: (125).toString(),
      total: (6 * 125).toString(),
    };
    iitem1 = insertInvoiceItemSchema.parse(iitem1);

    let iitem2: z.infer<typeof insertInvoiceItemSchema> = {
      id: 2,
      invoiceId: 1,
      title: "Manual task",
      quantity: (6.75).toString(),
      price: (125.79).toString(),
      total: (6.75 * 125.79).toString(),
    };
    iitem2 = insertInvoiceItemSchema.parse(iitem2);

    await tx.insert(bankAccounts).values([bankUS, bankEU]);
    await tx.insert(clients).values([clientUS, clientDE]);
    await tx.insert(issuers).values([issuerA, issuerB]);
    await tx.insert(tasks).values([task1, task2]);
    await tx.insert(taskTimes).values([time1, time2]);
    await tx.insert(invoices).values(invoice1);
    await tx.insert(invoiceItems).values([iitem1, iitem2]);
  });
};
