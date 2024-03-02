import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema } from "../schemas";
import { z } from "zod";
import { db } from "@db/index";
import { bankAccountCreateSchema, bankAccountSchema } from ".";
import { bankAccounts } from "@db/schemas";
import { insertBankAccountSchema } from "@db/schemas/bankAccounts";

export const create = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "POST",
    url: routes.bankAccounts,
    schema: {
      operationId: "create",
      description: "Create Bank Account",
      tags: [tags.bankAccount],
      body: bankAccountCreateSchema,
      response: {
        200: bankAccountSchema,
        400: badRequestSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      let newEntry: z.infer<typeof insertBankAccountSchema> = {
        name: req.body.name,
        info: req.body.info,
        currency: req.body.currency,
      };
      newEntry = insertBankAccountSchema.parse(newEntry);
      const dbResults = await db.insert(bankAccounts).values(newEntry).returning();

      const dbResult = bankAccountSchema.parse(dbResults[0])
      res.code(200).send(dbResult);
    },
  });
};