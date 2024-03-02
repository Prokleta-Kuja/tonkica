import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema } from "../schemas";
import { z } from "zod";
import { db } from "@db/index";
import { issuerCreateSchema, issuerSchema } from ".";
import { issuers } from "@db/schemas";
import { insertIssuerSchema } from "@db/schemas/issuers";

export const create = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "POST",
    url: routes.issuers,
    schema: {
      operationId: "create",
      description: "Create Issuer",
      tags: [tags.issuer],
      body: issuerCreateSchema,
      response: {
        200: issuerSchema,
        400: badRequestSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      let newEntry: z.infer<typeof insertIssuerSchema> = {
        name: req.body.name,
        info: req.body.info,
        currency: req.body.currency,
        tz: req.body.tz,
        locale: req.body.locale,
        invoicedBy: req.body.invoicedBy,
      };
      newEntry = insertIssuerSchema.parse(newEntry);
      const dbResults = await db.insert(issuers).values(newEntry).returning();

      const dbResult = issuerSchema.parse(dbResults[0])
      res.code(200).send(dbResult);
    },
  });
};