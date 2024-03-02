import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { db } from "@db/index";
import { and, asc, desc, count, ilike, or, SQL } from "drizzle-orm";
import { issuerOrderByMapping, issuersSchema, issuersQuerySchema, issuerSchema } from ".";
import { issuers } from "@db/schemas";

export const getAll = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.issuers,
    schema: {
      operationId: "getAll",
      description: "Get all Bank Accounts",
      tags: [tags.issuer],
      querystring: issuersQuerySchema,
      response: {
        200: issuersSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {

      const orderCol = issuerOrderByMapping[req.query.orderBy ?? "name"];

      const term = req.query.term?.trim();
      const search: (SQL | undefined)[] = [];
      if (term)
        term.split(" ").forEach((t) => {
          t = `%${t}%`;
          search.push(or(ilike(issuers.name, t),));
        });

      const where = and(...search);

      const [{ total }] = await db
        .select({ total: count() })
        .from(issuers)
        .where(where);

      const results =
        total === 0
          ? []
          : await db
            .select({
              name: issuers.name,
              info: issuers.info,
              currency: issuers.currency,
              tz: issuers.tz,
              locale: issuers.locale,
              invoicedBy: issuers.invoicedBy,
            })
            .from(issuers)
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
        items: results.map(r => issuerSchema.parse(r)),
      });
    },
  });
};
