import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema } from "../schemas";
import { z } from "zod";
import { db } from "@db/index";
import { clientCreateSchema, clientSchema } from ".";
import { clients } from "@db/schemas";
import { insertClientSchema } from "@db/schemas/clients";

export const create = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "POST",
    url: routes.clients,
    schema: {
      operationId: "create",
      description: "Create Client",
      tags: [tags.client],
      body: clientCreateSchema,
      response: {
        200: clientSchema,
        400: badRequestSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      let newEntry: z.infer<typeof insertClientSchema> = {
        name: req.body.name,
        info: req.body.info,
        currency: req.body.currency,
        rate: req.body.rate,
        daysDue: req.body.daysDue,
        tz: req.body.tz,
        locale: req.body.locale,
      };
      newEntry = insertClientSchema.parse(newEntry);
      const dbResults = await db.insert(clients).values(newEntry).returning();

      const dbResult = clientSchema.parse(dbResults[0])
      res.code(200).send(dbResult);
    },
  });
};