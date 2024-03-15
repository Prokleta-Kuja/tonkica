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
import { taskSchema, taskUpdateSchema } from ".";
import { clients, tasks } from "@db/schemas";
import { selectTaskSchema } from "@db/schemas/tasks";
import { nameof } from "@utils/index";
import { ZodError } from "zod";

export const update = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "PUT",
    url: routes.task,
    schema: {
      operationId: "update",
      description: "Update Task",
      tags: [tags.task],
      params: idParamSchema,
      body: taskUpdateSchema,
      response: {
        200: taskSchema,
        400: badRequestSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select()
        .from(tasks)
        .where(eq(tasks.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();
      const [dbResult] = dbResults;

      const client = await db.query.issuers.findFirst({
        where: eq(clients.id, req.body.clientId),
      });
      if (!client)
        throw new ZodError([
          {
            code: "custom",
            path: [nameof<z.infer<typeof taskUpdateSchema>>("clientId")],
            message: "Invalid client",
          },
        ]);

      const entryUpdate: Partial<z.infer<typeof selectTaskSchema>> = {
        title: req.body.title,
        clientId: req.body.clientId,
      };

      await db.transaction(async (tx) => {
        const updatedDbResults = await tx
          .update(tasks)
          .set(entryUpdate)
          .where(eq(tasks.id, dbResult.id))
          .returning();

        if (updatedDbResults.length > 1)
          throw new Error("Update affected multiple records");

        const updatedDbResult = taskSchema.parse(updatedDbResults[0]);
        res.code(200).send({ ...updatedDbResult, clientName: client.name });
      });
    },
  });
};
