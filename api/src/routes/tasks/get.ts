import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { idParamSchema, notFoundSchema } from "../schemas";
import { db } from "@db/index";
import { clients, tasks } from "@db/schemas";
import { eq } from "drizzle-orm";
import { taskSchema } from ".";

export const get = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.task,
    schema: {
      operationId: "get",
      description: "Get Task",
      tags: [tags.task],
      params: idParamSchema,
      response: {
        200: taskSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .select({
          id: tasks.id,
          clientId: tasks.clientId,
          clientName: clients.name,
          title: tasks.title,
          created: tasks.created,
        })
        .from(tasks)
        .innerJoin(clients, eq(clients.id, tasks.clientId))
        .where(eq(tasks.id, req.params.id))
        .limit(2);

      if (dbResults.length !== 1) return res.code(404).send();

      const dbResult = taskSchema.parse(dbResults[0]);
      res.code(200).send(dbResult);
    },
  });
};
