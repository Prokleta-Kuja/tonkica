import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema, idParamSchema, notFoundSchema } from "../schemas";
import { ZodError, z } from "zod";
import { db } from "@db/index";
import { count, eq } from "drizzle-orm";
import { invoiceSchema, invoiceUpdateSchema } from ".";
import { bankAccounts, clients, invoices, issuers } from "@db/schemas";
import { selectInvoiceSchema } from "@db/schemas/invoices";
import { nameof } from "@utils/index";

export const update = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "PUT",
    url: routes.invoice,
    schema: {
      operationId: "update",
      description: "Update Invoice",
      tags: [tags.invoice],
      params: idParamSchema,
      body: invoiceUpdateSchema,
      response: {
        200: invoiceSchema,
        400: badRequestSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select()
        .from(invoices)
        .where(eq(invoices.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();
      const [dbResult] = dbResults;

      const dbIssuers = await db
        .select()
        .from(issuers)
        .where(eq(issuers.id, req.body.issuerId))
        .limit(2);
      if (dbIssuers.length !== 1)
        throw new ZodError([
          {
            code: "custom",
            path: [nameof<z.infer<typeof invoiceUpdateSchema>>("issuerId")],
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
            path: [nameof<z.infer<typeof invoiceUpdateSchema>>("clientId")],
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
              nameof<z.infer<typeof invoiceUpdateSchema>>("bankAccountId"),
            ],
            message: "Invalid bank account",
          },
        ]);

      const [dbIssuer] = dbIssuers;
      const [dbClient] = dbClients;

      const entryUpdate: Partial<z.infer<typeof selectInvoiceSchema>> = {
        issuerId: req.body.issuerId,
        clientId: req.body.clientId,
        bankAccountId: req.body.bankAccountId,
        sequenceNumber: req.body.sequenceNumber,
        subject: req.body.subject,
        currency: req.body.currency,
        displayCurrency: req.body.displayCurrency,
        displayRate: req.body.displayRate,
        published: req.body.published,
        paid: req.body.paid,
        note: req.body.note,
      };

      await db.transaction(async (tx) => {
        const updatedDbResults = await tx
          .update(invoices)
          .set(entryUpdate)
          .where(eq(invoices.id, dbResult.id))
          .returning({ id: invoices.id });

        if (updatedDbResults.length > 1)
          throw new Error("Update affected multiple records");

        const [{ id }] = updatedDbResults;
        const updateResult = await db
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
          .where(eq(invoices.id, req.params.id))
          .limit(2);

        res.code(200).send(invoiceSchema.parse(updateResult));
      });
    },
  });
};
