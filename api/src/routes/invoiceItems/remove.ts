import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import {
  badRequestSchema,
  emptySchema,
  idParamSchema,
  notFoundSchema,
} from "../schemas";
import { db } from "@db/index";
import { eq } from "drizzle-orm";
import { invoiceItems } from "@db/schemas";

export const remove = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "DELETE",
    url: routes.invoice,
    schema: {
      operationId: "delete",
      description: "Delete Invoice Item",
      tags: [tags.invoice],
      params: idParamSchema,
      response: {
        204: emptySchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .delete(invoiceItems)
        .where(eq(invoiceItems.id, req.params.id))
        .returning({ id: invoiceItems.id });

      if (dbResults.length !== 1) return res.code(404).send();

      res.code(204).send();
    },
  });
};
