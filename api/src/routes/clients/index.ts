import { currencySchema } from "@db/enums";
import { clients } from "@db/schemas";
import { PgColumn } from "drizzle-orm/pg-core";
import { z } from "zod";
import { extendZodWithOpenApi } from "zod-openapi";
import { getListSchema, getpaginationQuerySchema } from "../schemas";
import { FastifyInstance } from "fastify";
import { create } from "./create";
import { get } from "./get";
import { getAll } from "./getAll";
import { update } from "./update";

extendZodWithOpenApi(z);
export const clientCreateSchema = z
  .object({
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    rate: z.number().positive(),
    daysDue: z.number().nonnegative().multipleOf(1, "Must be an integer"),
    tz: z.string(),
    locale: z.string(),
  })
  .openapi({ ref: "ClientCreate" });

export const clientUpdateSchema = z
  .object({
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    rate: z.number().positive(),
    daysDue: z.number().nonnegative().multipleOf(1, "Must be an integer"),
    tz: z.string(),
    locale: z.string(),
  })
  .openapi({ ref: "ClientUpdate" });

export const clientSchema = z
  .object({
    id: z.number(),
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    rate: z.number().positive(),
    daysDue: z.number().nonnegative().multipleOf(1, "Must be an integer"),
    tz: z.string(),
    locale: z.string(),
  })
  .openapi({ ref: "ClientUpdate" });

export const clientOrderBy = z
  .enum(["name", "currency", "rate", "due"])
  .openapi({ ref: "ClientOrderBy" });
export const clientOrderByMapping: Record<
  z.infer<typeof clientOrderBy>,
  PgColumn
> = {
  name: clients.name,
  currency: clients.currency,
  rate: clients.rate,
  due: clients.daysDue,
};
export const clientsQuerySchema = getpaginationQuerySchema(clientOrderBy);
export const clientsSchema = getListSchema(clientSchema);

export const registerClientRoutes = async (
  fastify: FastifyInstance,
  options: Object
) => {
  create(fastify, options);
  get(fastify, options);
  getAll(fastify, options);
  update(fastify, options);
};
