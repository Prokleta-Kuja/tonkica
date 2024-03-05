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
import { invoiceItemSchema, invoiceItemUpdateSchema } from ".";
import { bankAccounts, clients, invoices, issuers } from "@db/schemas";
import {
  invoiceItems,
  selectInvoiceItemSchema,
} from "@db/schemas/invoiceItems";
import { nameof } from "@utils/index";

export const update = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "PUT",
    url: routes.invoiceItem,
    schema: {
      operationId: "update",
      description: "Update Invoice Item",
      tags: [tags.invoice],
      params: idParamSchema,
      body: invoiceItemUpdateSchema,
      response: {
        200: invoiceItemSchema,
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

      const entryUpdate: Partial<z.infer<typeof selectInvoiceItemSchema>> = {
        title: req.body.title,
        quantity: req.body.quantity,
        price: req.body.price,
      };

      await db.transaction(async (tx) => {
        const updatedDbResults = await tx
          .update(invoiceItems)
          .set(entryUpdate)
          .where(eq(invoiceItems.id, dbResult.id))
          .returning({ id: invoiceItems.id });

        if (updatedDbResults.length > 1)
          throw new Error("Update affected multiple records");

        const [{ id }] = updatedDbResults;
        const updateResult = await db
          .select({
            title: invoiceItems.title,
            quantity: invoiceItems.quantity,
            price: invoiceItems.price,
            total: invoiceItems.total,
          })
          .from(invoiceItems)
          .where(eq(invoices.id, req.params.id))
          .limit(2);

        res.code(200).send(invoiceItemSchema.parse({ id, ...updateResult }));
      });
    },
  });
};
