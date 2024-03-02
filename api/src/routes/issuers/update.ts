import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema, idParamSchema } from "../schemas";
import { z } from "zod";
import { db } from "@db/index";
import { eq } from "drizzle-orm";
import { issuerSchema, issuerUpdateSchema } from ".";
import { issuers } from "@db/schemas";
import { selectIssuerSchema } from "@db/schemas/issuers";

export const update = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "PUT",
    url: routes.issuer,
    schema: {
      operationId: "update",
      description: "Update Bank Account",
      tags: [tags.issuer],
      params: idParamSchema,
      body: issuerUpdateSchema,
      response: {
        200: issuerSchema,
        400: badRequestSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select()
        .from(issuers)
        .where(eq(issuers.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();
      const [dbResult] = dbResults;

      const entryUpdate: Partial<z.infer<typeof selectIssuerSchema>> = {
        name: req.body.name,
        info: req.body.info,
        currency: req.body.currency,
        tz: req.body.tz,
        locale: req.body.locale,
        invoicedBy: req.body.invoicedBy,
      };

      await db.transaction(async (tx) => {
        const updatedDbResults = await tx
          .update(issuers)
          .set(entryUpdate)
          .where(eq(issuers.id, dbResult.id))
          .returning();

        if (updatedDbResults.length > 1)
          throw new Error("Update affected multiple records");

        const updatedDbResult = issuerSchema.parse(updatedDbResults[0])
        res.code(200).send(updatedDbResult);
      })
    },
  });
};
