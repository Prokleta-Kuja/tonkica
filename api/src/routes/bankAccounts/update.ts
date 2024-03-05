import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema, idParamSchema, notFoundSchema } from "../schemas";
import { z } from "zod";
import { db } from "@db/index";
import { eq } from "drizzle-orm";
import { bankAccountSchema, bankAccountUpdateSchema } from ".";
import { bankAccounts } from "@db/schemas";
import { selectBankAccountSchema } from "@db/schemas/bankAccounts";

export const update = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "PUT",
    url: routes.bankAccount,
    schema: {
      operationId: "update",
      description: "Update Bank Account",
      tags: [tags.bankAccount],
      params: idParamSchema,
      body: bankAccountUpdateSchema,
      response: {
        200: bankAccountSchema,
        400: badRequestSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select()
        .from(bankAccounts)
        .where(eq(bankAccounts.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();
      const [dbResult] = dbResults;

      const entryUpdate: Partial<z.infer<typeof selectBankAccountSchema>> = {
        name: req.body.name,
        info: req.body.info,
        currency: req.body.currency,
      };

      await db.transaction(async (tx) => {
        const updatedDbResults = await tx
          .update(bankAccounts)
          .set(entryUpdate)
          .where(eq(bankAccounts.id, dbResult.id))
          .returning();

        if (updatedDbResults.length > 1)
          throw new Error("Update affected multiple records");

        const updatedDbResult = bankAccountSchema.parse(updatedDbResults[0]);
        res.code(200).send(updatedDbResult);
      });
    },
  });
};
