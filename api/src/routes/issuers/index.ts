import { currencySchema } from "@db/enums";
import { issuers } from "@db/schemas";
import { PgColumn } from "drizzle-orm/pg-core";
import { z } from "zod";
import { extendZodWithOpenApi } from "zod-openapi";
import { getListSchema, getpaginationQuerySchema } from "../schemas";
import { FastifyInstance } from "fastify";

extendZodWithOpenApi(z);
export const issuerCreateSchema = z
  .object({
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    tz: z.string(),
    locale: z.string(),
    invoicedBy: z.string(),
    // TODO: logo
  })
  .openapi({ ref: "IssuerCreate" });

export const issuerUpdateSchema = z
  .object({
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    tz: z.string(),
    locale: z.string(),
    invoicedBy: z.string(),
    // TODO: logo
  })
  .openapi({ ref: "IssuerUpdate" });

export const issuerSchema = z
  .object({
    id: z.number(),
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    tz: z.string(),
    locale: z.string(),
    invoicedBy: z.string(),
    // TODO: logo
  })
  .openapi({ ref: "IssuerUpdate" });

export const issuerOrderBy = z
  .enum(["name", "currency"])
  .openapi({ ref: "IssuerOrderBy" });
export const issuerOrderByMapping: Record<
  z.infer<typeof issuerOrderBy>,
  PgColumn
> = {
  name: issuers.name,
  currency: issuers.currency,
};
export const issuersQuerySchema = getpaginationQuerySchema(issuerOrderBy);
export const issuersSchema = getListSchema(issuerSchema);

export const registerIssuerRoutes = async (
  fastify: FastifyInstance,
  options: Object
) => {
  getAll(fastify, options);
};
