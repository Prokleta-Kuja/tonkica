import { FastifyInstance } from "fastify";
import {
  FastifyZodOpenApiSchema,
  FastifyZodOpenApiTypeProvider,
} from "fastify-zod-openapi";
import { routes, tags } from "..";
import { db } from "@db/index";
import { and, asc, desc, count, ilike, or, SQL, eq } from "drizzle-orm";
import {
  invoiceitemOrderByMapping,
  invoiceItemsSchema,
  invoiceItemsQuerySchema,
  invoiceItemSchema,
} from ".";
import { invoiceItems } from "@db/schemas";

export const getAll = async (fastify: FastifyInstance, _options: Object) => {
  fastify.withTypeProvider<FastifyZodOpenApiTypeProvider>().route({
    method: "GET",
    url: routes.invoiceItems,
    schema: {
      operationId: "getAll",
      description: "Get all Invoice Items",
      tags: [tags.invoiceItem],
      querystring: invoiceItemsQuerySchema,
      response: {
        200: invoiceItemsSchema,
      },
    } satisfies FastifyZodOpenApiSchema,
    handler: async (req, res) => {
      const orderCol = invoiceitemOrderByMapping[req.query.orderBy ?? "id"];

      const term = req.query.term?.trim();
      const search: (SQL | undefined)[] = [];
      if (term)
        term.split(" ").forEach((t) => {
          t = `%${t}%`;
          search.push(or(ilike(invoiceItems.title, t)));
        });

      const where = and(...search);

      const [{ total }] = await db
        .select({ total: count() })
        .from(invoiceItems)
        .where(where);

      const results =
        total === 0
          ? []
          : await db
              .select({
                title: invoiceItems.title,
                quantity: invoiceItems.quantity,
                price: invoiceItems.price,
                total: invoiceItems.total,
              })
              .from(invoiceItems)
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
        items: results.map((r) => invoiceItemSchema.parse(r)),
      });
    },
  });
};
