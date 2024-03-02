import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { idParamSchema, notFoundSchema } from "../schemas";
import { db } from "@db/index";
import { issuers } from "@db/schemas";
import { eq } from "drizzle-orm";
import { issuerSchema } from ".";

export const get = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.issuer,
    schema: {
      operationId: "get",
      description: "Get Bank Account",
      tags: [tags.issuer],
      params: idParamSchema,
      response: {
        200: issuerSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select({
          name: issuers.name,
          info: issuers.info,
          currency: issuers.currency,
          tz: issuers.tz,
          locale: issuers.locale,
          invoicedBy: issuers.invoicedBy,
        })
        .from(issuers)
        .where(eq(issuers.id, req.params.id)
        )
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();

      const dbResult = issuerSchema.parse(dbResults[0])
      res.code(200).send(dbResult);
    },
  });
};