import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema } from "../schemas";
import { ZodError, z } from "zod";
import { db } from "@db/index";
import { bankAccountCreateSchema, bankAccountSchema } from ".";
import { bankAccounts, issuers } from "@db/schemas";
import { insertBankAccountSchema } from "@db/schemas/bankAccounts";
import { eq } from "drizzle-orm";
import { nameof } from "@utils/index";

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
      const issuer = await db.query.issuers.findFirst({
        where: eq(issuers.id, req.body.issuerId),
      });
      if (!issuer)
        throw new ZodError([
          {
            code: "custom",
            path: [nameof<z.infer<typeof insertBankAccountSchema>>("issuerId")],
            message: "Invalid issuer",
          },
        ]);

      let newEntry: z.infer<typeof insertBankAccountSchema> = {
        name: req.body.name,
        info: req.body.info,
        currency: req.body.currency,
        issuerId: issuer.id,
      };
      newEntry = insertBankAccountSchema.parse(newEntry);
      const dbResults = await db
        .insert(bankAccounts)
        .values(newEntry)
        .returning();

      const dbResult = bankAccountSchema.parse(dbResults[0]);
      res.code(200).send(dbResult);
    },
  });
};
