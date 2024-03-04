import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { db } from "@db/index";
import { and, asc, desc, count, ilike, or, SQL, eq } from "drizzle-orm";
import {
  invoiceOrderByMapping,
  invoicesSchema,
  invoicesQuerySchema,
  invoiceSchema,
} from ".";
import { bankAccounts, clients, invoices, issuers } from "@db/schemas";

export const getAll = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.invoices,
    schema: {
      operationId: "getAll",
      description: "Get all Invoices",
      tags: [tags.invoice],
      querystring: invoicesQuerySchema,
      response: {
        200: invoicesSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const orderCol = invoiceOrderByMapping[req.query.orderBy ?? "published"];

      const term = req.query.term?.trim();
      const search: (SQL | undefined)[] = [];
      if (term)
        term.split(" ").forEach((t) => {
          t = `%${t}%`;
          search.push(or(ilike(issuers.name, t)));
          search.push(or(ilike(clients.name, t)));
          search.push(or(ilike(invoices.sequenceNumber, t)));
          search.push(or(ilike(invoices.subject, t)));
        });

      const where = and(...search);

      const [{ total }] = await db
        .select({ total: count() })
        .from(invoices)
        .where(where);

      const results =
        total === 0
          ? []
          : await db
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
              .innerJoin(
                clients,
                eq(clients.id, eq(clients.id, invoices.clientId))
              )
              .innerJoin(
                bankAccounts,
                eq(bankAccounts.id, invoices.bankAccountId)
              )
              .where(where)
              .orderBy(req.query.orderAsc ? asc(orderCol) : desc(orderCol))
              .limit(req.query.size)
              .offset((req.query.page - 1) * req.query.size);

      res.code(200).send({
        page: req.query.page,
        size: req.query.size,
        orderBy: req.query.orderBy,
        orderAsc: req.query.orderAsc,
        total,
        items: results.map((r) => invoiceSchema.parse(r)),
      });
    },
  });
};
