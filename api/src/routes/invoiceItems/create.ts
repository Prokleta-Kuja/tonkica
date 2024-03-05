import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema, idParamSchema, notFoundSchema } from "../schemas";
import { z } from "zod";
import { db } from "@db/index";
import { invoiceItemCreateSchema, invoiceItemSchema } from ".";
import { invoiceItems, invoices } from "@db/schemas";
import { insertInvoiceItemSchema } from "@db/schemas/invoiceItems";
import { eq } from "drizzle-orm";
import { dbRound } from "@utils/index";

export const create = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "POST",
    url: routes.invoiceItems,
    schema: {
      operationId: "create",
      description: "Create Invoice Item",
      tags: [tags.invoiceItem],
      params: idParamSchema,
      body: invoiceItemCreateSchema,
      response: {
        200: invoiceItemSchema,
        400: badRequestSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbInvoices = await db
        .select()
        .from(invoices)
        .where(eq(invoices.id, req.params.id))
        .limit(2);
      if (dbInvoices.length !== 1) return res.code(404).send();
      const [dbInvoice] = dbInvoices;

      let newEntry: z.infer<typeof insertInvoiceItemSchema> = {
        invoiceId: dbInvoice.id,
        title: req.body.title,
        quantity: req.body.quantity,
        price: req.body.price,
        total: dbRound(req.body.quantity * req.body.price),
      };
      newEntry = insertInvoiceItemSchema.parse(newEntry);
      const [dbResult] = await db
        .insert(invoiceItems)
        .values(newEntry)
        .returning();

      res.code(200).send(invoiceItemSchema.parse(dbResult));
    },
  });
};
