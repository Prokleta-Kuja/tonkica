import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { idParamSchema, notFoundSchema } from "../schemas";
import { db } from "@db/index";
import { clients } from "@db/schemas";
import { eq } from "drizzle-orm";
import { clientSchema } from ".";

export const get = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.client,
    schema: {
      operationId: "get",
      description: "Get Client",
      tags: [tags.client],
      params: idParamSchema,
      response: {
        200: clientSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select({
          name: clients.name,
          info: clients.info,
          currency: clients.currency,
          rate: clients.rate,
          daysDue: clients.daysDue,
          tz: clients.tz,
          locale: clients.locale,
        })
        .from(clients)
        .where(eq(clients.id, req.params.id)
        )
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();

      const dbResult = clientSchema.parse(dbResults[0])
      res.code(200).send(dbResult);
    },
  });
};