import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { db } from "@db/index";
import { and, asc, desc, count, ilike, or, SQL } from "drizzle-orm";
import { clientOrderByMapping, clientsSchema, clientsQuerySchema, clientSchema } from ".";
import { clients } from "@db/schemas";

export const getAll = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.clients,
    schema: {
      operationId: "getAll",
      description: "Get all Clients",
      tags: [tags.client],
      querystring: clientsQuerySchema,
      response: {
        200: clientsSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {

      const orderCol = clientOrderByMapping[req.query.orderBy ?? "name"];

      const term = req.query.term?.trim();
      const search: (SQL | undefined)[] = [];
      if (term)
        term.split(" ").forEach((t) => {
          t = `%${t}%`;
          search.push(or(ilike(clients.name, t),));
        });

      const where = and(...search);

      const [{ total }] = await db
        .select({ total: count() })
        .from(clients)
        .where(where);

      const results =
        total === 0
          ? []
          : await db
            .select({
              name: clients.name,
              info: clients.info,
              currency: clients.currency,
              rate: clients.rate,
              daysDue: clients.daysDue,
              tz: clients.tz,
              locale: clients.locale,
            })
            .from(clients)
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
        items: results.map(r => clientSchema.parse(r)),
      });
    },
  });
};
