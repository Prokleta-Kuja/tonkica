import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { emptySchema, id2ParamSchema, notFoundSchema } from "../schemas";
import { db } from "@db/index";
import { and, eq } from "drizzle-orm";
import { taskTimes, tasks } from "@db/schemas";

export const remove = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "DELETE",
    url: routes.taskTime,
    schema: {
      operationId: "delete",
      description: "Delete Task Time",
      tags: [tags.taskTime],
      params: id2ParamSchema,
      response: {
        204: emptySchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbResults = await db
        .delete(taskTimes)
        .where(
          and(eq(tasks.id, req.params.id), eq(taskTimes.id, req.params.id2))
        )
        .returning({ id: taskTimes.id });

      if (dbResults.length !== 1) return res.code(404).send();

      res.code(204).send();
    },
  });
};
