import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { db } from "@db/index";
import { and, asc, desc, count, ilike, or, SQL } from "drizzle-orm";
import { bankAccountOrderByMapping, bankAccountsSchema, bankAccountsQuerySchema, bankAccountSchema } from ".";
import { bankAccounts } from "@db/schemas";

export const getAll = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.bankAccounts,
    schema: {
      operationId: "getAll",
      description: "Get all Bank Accounts",
      tags: [tags.bankAccount],
      querystring: bankAccountsQuerySchema,
      response: {
        200: bankAccountsSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {

      const orderCol = bankAccountOrderByMapping[req.query.orderBy ?? "name"];

      const term = req.query.term?.trim();
      const search: (SQL | undefined)[] = [];
      if (term)
        term.split(" ").forEach((t) => {
          t = `%${t}%`;
          search.push(or(ilike(bankAccounts.name, t),));
        });

      const where = and(...search);

      const [{ total }] = await db
        .select({ total: count() })
        .from(bankAccounts)
        .where(where);

      const results =
        total === 0
          ? []
          : await db
            .select({
              id: bankAccounts.id,
              name: bankAccounts.name,
              currency: bankAccounts.currency,
            })
            .from(bankAccounts)
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
        items: results.map(r => bankAccountSchema.parse(r)),
      });
    },
  });
};
