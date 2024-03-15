import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema, idParamSchema, notFoundSchema } from "../schemas";
import { z } from "zod";
import { db } from "@db/index";
import { taskTimeCreateSchema, taskTimeSchema } from ".";
import { taskTimes, invoices, tasks } from "@db/schemas";
import { insertTaskTimeSchema } from "@db/schemas/taskTimes";
import { eq } from "drizzle-orm";
import { dbRound } from "@utils/index";

export const create = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "POST",
    url: routes.taskTimes,
    schema: {
      operationId: "create",
      description: "Create Task Time",
      tags: [tags.taskTime],
      params: idParamSchema,
      body: taskTimeCreateSchema,
      response: {
        200: taskTimeSchema,
        400: badRequestSchema,
        404: notFoundSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const dbTasks = await db
        .select()
        .from(tasks)
        .where(eq(tasks.id, req.params.id))
        .limit(2);
      if (dbTasks.length !== 1) return res.code(404).send();
      const [dbTask] = dbTasks;

      let newEntry: z.infer<typeof insertTaskTimeSchema> = {
        taskId: dbTask.id,
        start: req.body.start,
        durationMs: req.body.durationMs,
      };
      newEntry = insertTaskTimeSchema.parse(newEntry);
      const [dbResult] = await db
        .insert(taskTimes)
        .values(newEntry)
        .returning();

      res
        .code(200)
        .send(taskTimeSchema.parse({ ...dbResult, taskTitle: dbTask.title }));
    },
  });
};
