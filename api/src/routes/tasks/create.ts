import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { badRequestSchema } from "../schemas";
import { ZodError, z } from "zod";
import { db } from "@db/index";
import { taskCreateSchema, taskSchema } from ".";
import { tasks, clients } from "@db/schemas";
import { insertTaskSchema } from "@db/schemas/tasks";
import { eq } from "drizzle-orm";
import { nameof } from "@utils/index";

export const create = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "POST",
    url: routes.tasks,
    schema: {
      operationId: "create",
      description: "Create Task",
      tags: [tags.task],
      body: taskCreateSchema,
      response: {
        200: taskSchema,
        400: badRequestSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const client = await db.query.issuers.findFirst({
        where: eq(clients.id, req.body.clientId),
      });
      if (!client)
        throw new ZodError([
          {
            code: "custom",
            path: [nameof<z.infer<typeof insertTaskSchema>>("clientId")],
            message: "Invalid client",
          },
        ]);

      let newEntry: z.infer<typeof insertTaskSchema> = {
        title: req.body.title,
        clientId: client.id,
      };
      newEntry = insertTaskSchema.parse(newEntry);
      const dbResults = await db.insert(tasks).values(newEntry).returning();

      const dbResult = taskSchema.parse(dbResults[0]);
      res.code(200).send(dbResult);
    },
  });
};
