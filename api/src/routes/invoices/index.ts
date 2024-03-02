import { currencySchema } from "@db/enums";
import { clients, invoices, issuers } from "@db/schemas";
import { PgColumn } from "drizzle-orm/pg-core";
import { z } from "zod";
import { extendZodWithOpenApi } from "zod-openapi";
import { getListSchema, getpaginationQuerySchema } from "../schemas";
import { FastifyInstance } from "fastify";
import { create } from "./create";
import { get } from "./get";
import { getAll } from "./getAll";
import { update } from "./update";

extendZodWithOpenApi(z);
export const invoiceCreateSchema = z
  .object({
    issuerId: z.number(),
    clientId: z.number(),
    bankAccountId: z.number(),
    sequenceNumber: z.string(),
    subject: z.string(),
  })
  .openapi({ ref: "InvoiceCreate" });

export const invoiceUpdateSchema = z
  .object({
    issuerId: z.number(),
    clientId: z.number(),
    bankAccountId: z.number(),
    sequenceNumber: z.string(),
    subject: z.string(),
    currency: currencySchema,
    displayCurrency: currencySchema,
    displayRate: z.number(),
    published: z.date(),
    paid: z.date().nullish(),
    note: z.string().nullish(),
  })
  .openapi({ ref: "InvoiceUpdate" });

export const invoiceSchema = z
  .object({
    id: z.number(),
    issuerId: z.number(),
    issuerName: z.string(),
    clientId: z.number(),
    clientName: z.string(),
    bankAccountId: z.number(),
    bankAccountName: z.string(),
    sequenceNumber: z.string(),
    subject: z.string(),
    currency: currencySchema,
    displayCurrency: currencySchema,
    displayRate: z.number(),
    published: z.date(),
    paid: z.date().nullish(),
    note: z.string().nullish(),
  })
  .openapi({ ref: "InvoiceUpdate" });

export const invoiceOrderBy = z
  .enum(["issuer", "client", "sequence", "subject", "published", "paid"])
  .openapi({ ref: "InvoiceOrderBy" });
export const invoiceOrderByMapping: Record<
  z.infer<typeof invoiceOrderBy>,
  PgColumn
> = {
  issuer: issuers.name,
  client: clients.name,
  sequence: invoices.sequenceNumber,
  subject: invoices.subject,
  published: invoices.published,
  paid: invoices.paid,
};
export const invoicesQuerySchema = getpaginationQuerySchema(invoiceOrderBy);
export const invoicesSchema = getListSchema(invoiceSchema);

export const registerInvoiceRoutes = async (
  fastify: FastifyInstance,
  options: Object
) => {
  create(fastify, options);
  get(fastify, options);
  getAll(fastify, options);
  update(fastify, options);
};
