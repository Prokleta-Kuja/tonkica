import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { idParamSchema, notFoundSchema } from "../schemas";
import { db } from "@db/index";
import { bankAccounts } from "@db/schemas";
import { eq } from "drizzle-orm";
import { bankAccountSchema } from ".";

export const get = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.bankAccount,
    schema: {
      operationId: "get",
      description: "Get Bank Account",
      tags: [tags.bankAccount],
      params: idParamSchema,
      response: {
        200: bankAccountSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select({
          id: bankAccounts.id,
          name: bankAccounts.name,
          info: bankAccounts.info,
          currency: bankAccounts.currency,
        })
        .from(bankAccounts)
        .where(eq(bankAccounts.id, req.params.id)
        )
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();

      const dbResult = bankAccountSchema.parse(dbResults[0])
      res.code(200).send(dbResult);
    },
  });
};