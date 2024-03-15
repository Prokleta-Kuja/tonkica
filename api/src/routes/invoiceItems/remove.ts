import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { emptySchema, id2ParamSchema, notFoundSchema } from "../schemas";
import { db } from "@db/index";
import { and, eq } from "drizzle-orm";
import { invoiceItems, invoices } from "@db/schemas";

export const remove = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "DELETE",
    url: routes.invoiceItem,
    schema: {
      operationId: "delete",
      description: "Delete Invoice Item",
      tags: [tags.invoice],
      params: id2ParamSchema,
      response: {
        204: emptySchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .delete(invoiceItems)
        .where(
          and(eq(invoices.id, req.params.id), eq(invoiceItems, req.params.id2))
        )
        .returning({ id: invoiceItems.id });

      if (dbResults.length !== 1) return res.code(404).send();

      res.code(204).send();
    },
  });
};
