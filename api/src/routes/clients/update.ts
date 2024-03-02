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
import { clientSchema, clientUpdateSchema } from ".";
import { clients } from "@db/schemas";
import { selectClientSchema } from "@db/schemas/clients";

export const update = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "PUT",
    url: routes.client,
    schema: {
      operationId: "update",
      description: "Update Client",
      tags: [tags.client],
      params: idParamSchema,
      body: clientUpdateSchema,
      response: {
        200: clientSchema,
        400: badRequestSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select()
        .from(clients)
        .where(eq(clients.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();
      const [dbResult] = dbResults;

      const entryUpdate: Partial<z.infer<typeof selectClientSchema>> = {
        name: req.body.name,
        info: req.body.info,
        currency: req.body.currency,
        rate: req.body.rate,
        daysDue: req.body.daysDue,
        tz: req.body.tz,
        locale: req.body.locale,
      };

      await db.transaction(async (tx) => {
        const updatedDbResults = await tx
          .update(clients)
          .set(entryUpdate)
          .where(eq(clients.id, dbResult.id))
          .returning();

        if (updatedDbResults.length > 1)
          throw new Error("Update affected multiple records");

        const updatedDbResult = clientSchema.parse(updatedDbResults[0])
        res.code(200).send(updatedDbResult);
      })
    },
  });
};
