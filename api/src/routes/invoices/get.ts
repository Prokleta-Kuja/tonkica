import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { idParamSchema, notFoundSchema } from "../schemas";
import { db } from "@db/index";
import { bankAccounts, clients, invoices, issuers } from "@db/schemas";
import { eq } from "drizzle-orm";
import { invoiceSchema } from ".";

export const get = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.invoice,
    schema: {
      operationId: "get",
      description: "Get Invoice",
      tags: [tags.invoice],
      params: idParamSchema,
      response: {
        200: invoiceSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
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
        .where(eq(invoices.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();

      const dbResult = invoiceSchema.parse(dbResults[0]);
      res.code(200).send(dbResult);
    },
  });
};
