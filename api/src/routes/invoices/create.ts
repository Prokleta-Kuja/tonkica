import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema } from "../schemas";
import { ZodError, z } from "zod";
import { db } from "@db/index";
import { invoiceCreateSchema, invoiceSchema } from ".";
import { bankAccounts, clients, invoices, issuers } from "@db/schemas";
import { insertInvoiceSchema } from "@db/schemas/invoices";
import { count, eq } from "drizzle-orm";
import { nameof } from "@utils/index";

export const create = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "POST",
    url: routes.invoices,
    schema: {
      operationId: "create",
      description: "Create Invoice",
      tags: [tags.invoice],
      body: invoiceCreateSchema,
      response: {
        200: invoiceSchema,
        400: badRequestSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbIssuers = await db
        .select()
        .from(issuers)
        .where(eq(issuers.id, req.body.issuerId))
        .limit(2);
      if (dbIssuers.length !== 1)
        throw new ZodError([
          {
            code: "custom",
            path: [nameof<z.infer<typeof invoiceCreateSchema>>("issuerId")],
            message: "Invalid issuer",
          },
        ]);

      const dbClients = await db
        .select()
        .from(clients)
        .where(eq(clients.id, req.body.clientId))
        .limit(2);
      if (dbClients.length !== 1)
        throw new ZodError([
          {
            code: "custom",
            path: [nameof<z.infer<typeof invoiceCreateSchema>>("clientId")],
            message: "Invalid client",
          },
        ]);

      const [{ bankAccCount }] = await db
        .select({ bankAccCount: count().mapWith(Number) })
        .from(bankAccounts)
        .where(eq(bankAccounts.id, req.body.bankAccountId));
      if (bankAccCount !== 1)
        throw new ZodError([
          {
            code: "custom",
            path: [
              nameof<z.infer<typeof invoiceCreateSchema>>("bankAccountId"),
            ],
            message: "Invalid bank account",
          },
        ]);

      const [dbIssuer] = dbIssuers;
      const [dbClient] = dbClients;
      let newEntry: z.infer<typeof insertInvoiceSchema> = {
        issuerId: req.body.issuerId,
        clientId: req.body.clientId,
        bankAccountId: req.body.bankAccountId,
        sequenceNumber: req.body.sequenceNumber,
        subject: req.body.subject,
        currency: dbIssuer.currency,
        displayCurrency: dbClient.currency,
        displayRate: 1,
        published: new Date(),
      };
      newEntry = insertInvoiceSchema.parse(newEntry);
      const [{ id }] = await db
        .insert(invoices)
        .values(newEntry)
        .returning({ id: invoices.id });

      const dbResults = await db
        .select({
          id: invoices.id,
          issuerId: invoices.issuerId,
          issuerName: issuers.name,
          clientId: invoices.clientId,
          clientName: clients.name,
          bankAccountId: invoices.bankAccountId,
          bankAccountName: bankAccounts.name,
          sequenceNumber: invoices.sequenceNumber,
          subject: invoices.subject,
          currency: invoices.currency,
          displayCurrency: invoices.displayCurrency,
          displayRate: invoices.displayRate,
          published: invoices.published,
          paid: invoices.paid,
          note: invoices.note,
        })
        .from(invoices)
        .innerJoin(issuers, eq(issuers.id, invoices.issuerId))
        .innerJoin(clients, eq(clients.id, eq(clients.id, invoices.clientId)))
        .innerJoin(bankAccounts, eq(bankAccounts.id, invoices.bankAccountId))
        .where(eq(invoices.id, id))
        .limit(2);

      const dbResult = invoiceSchema.parse(dbResults[0]);
      res.code(200).send(dbResult);
    },
  });
};
