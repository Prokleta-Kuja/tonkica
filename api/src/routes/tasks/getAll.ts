import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { db } from "@db/index";
import { and, asc, desc, count, ilike, or, SQL, eq } from "drizzle-orm";
import {
  taskOrderByMapping,
  tasksSchema,
  tasksQuerySchema,
  taskSchema,
} from ".";
import { clients, tasks } from "@db/schemas";

export const getAll = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.tasks,
    schema: {
      operationId: "getAll",
      description: "Get all Tasks",
      tags: [tags.task],
      querystring: tasksQuerySchema,
      response: {
        200: tasksSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const orderCol = taskOrderByMapping[req.query.orderBy ?? "created"];

      const term = req.query.term?.trim();
      const search: (SQL | undefined)[] = [];
      if (term)
        term.split(" ").forEach((t) => {
          t = `%${t}%`;
          search.push(or(ilike(tasks.title, t)));
        });

      const where = and(...search);

      const [{ total }] = await db
        .select({ total: count() })
        .from(tasks)
        .where(where);

      const results =
        total === 0
          ? []
          : await db
              .select({
                id: tasks.id,
                clientId: tasks.clientId,
                clientName: clients.name,
                title: tasks.title,
                created: tasks.created,
              })
              .from(tasks)
              .innerJoin(clients, eq(clients.id, tasks.clientId))
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
        items: results.map((r) => taskSchema.parse(r)),
      });
    },
  });
};
