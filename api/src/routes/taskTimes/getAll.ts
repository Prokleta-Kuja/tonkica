import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { db } from "@db/index";
import { and, asc, desc, count, ilike, or, SQL, eq } from "drizzle-orm";
import {
  tasktimeOrderByMapping,
  taskTimesSchema,
  taskTimesQuerySchema,
  taskTimeSchema,
} from ".";
import { taskTimes, tasks } from "@db/schemas";

export const getAll = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.taskTimes,
    schema: {
      operationId: "getAll",
      description: "Get all Task Times",
      tags: [tags.taskTime],
      querystring: taskTimesQuerySchema,
      response: {
        200: taskTimesSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const orderCol = tasktimeOrderByMapping[req.query.orderBy ?? "start"];

      const term = req.query.term?.trim();
      const search: (SQL | undefined)[] = [];
      // if (term)
      //   term.split(" ").forEach((t) => {
      //     t = `%${t}%`;
      //     search.push(or(ilike(taskTimes., t)));
      //   });

      const where = and(...search);

      const [{ total }] = await db
        .select({ total: count() })
        .from(taskTimes)
        .where(where);

      const results =
        total === 0
          ? []
          : await db
              .select({
                taskTitle: tasks.title,
                start: taskTimes.start,
                durationMs: taskTimes.durationMs,
              })
              .from(taskTimes)
              .innerJoin(tasks, eq(tasks.id, taskTimes.taskId))
              .where(where)
              .orderBy(req.query.orderAsc ? asc(orderCol) : desc(orderCol))
              .limit(req.query.size)
              .offset((req.query.page - 1) * req.query.size);

      res.code(200).send({
        page: req.query.page,
        size: req.query.size,
        orderBy: req.query.orderBy,
        orderAsc: req.query.orderAsc,
        total,
        items: results.map((r) => taskTimeSchema.parse(r)),
      });
    },
  });
};
