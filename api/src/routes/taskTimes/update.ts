import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema, idParamSchema, notFoundSchema } from "../schemas";
import { ZodError, z } from "zod";
import { db } from "@db/index";
import { count, eq } from "drizzle-orm";
import { taskTimeSchema, taskTimeUpdateSchema } from ".";
import { bankAccounts, clients, invoices, issuers } from "@db/schemas";
import { taskTimes, selectTaskTimeSchema } from "@db/schemas/taskTimes";
import { nameof } from "@utils/index";

export const update = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "PUT",
    url: routes.taskTime,
    schema: {
      operationId: "update",
      description: "Update Task Time",
      tags: [tags.invoice],
      params: idParamSchema,
      body: taskTimeUpdateSchema,
      response: {
        200: taskTimeSchema,
        400: badRequestSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select()
        .from(taskTimes)
        .where(eq(taskTimes.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();
      const [dbResult] = dbResults;

      const entryUpdate: Partial<z.infer<typeof selectTaskTimeSchema>> = {
        start: req.body.start,
        durationMs: req.body.durationMs,
      };

      await db.transaction(async (tx) => {
        const updatedDbResults = await tx
          .update(taskTimes)
          .set(entryUpdate)
          .where(eq(taskTimes.id, dbResult.id))
          .returning({ id: taskTimes.id });

        if (updatedDbResults.length > 1)
          throw new Error("Update affected multiple records");

        const [updatedResult] = updatedDbResults;
        res.code(200).send(taskTimeSchema.parse(updatedResult));
      });
    },
  });
};
