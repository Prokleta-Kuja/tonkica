import { currencySchema } from "@db/enums";
import { bankAccounts } from "@db/schemas";
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
export const bankAccountCreateSchema = z
  .object({
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    issuerId: z.number().gt(0),
  })
  .openapi({ ref: "BankAccountCreate" });

export const bankAccountUpdateSchema = z
  .object({
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
    issuerId: z.number().gt(0),
  })
  .openapi({ ref: "BankAccountUpdate" });

export const bankAccountSchema = z
  .object({
    id: z.number(),
    issuerId: z.number(),
    issuerName: z.string().min(2).max(64),
    name: z.string().min(2).max(64),
    info: z.string().min(2).max(256),
    currency: currencySchema,
  })
  .openapi({ ref: "BankAccountUpdate" });

export const bankAccountOrderBy = z
  .enum(["name", "currency"])
  .openapi({ ref: "BankAccountOrderBy" });
export const bankAccountOrderByMapping: Record<
  z.infer<typeof bankAccountOrderBy>,
  PgColumn
> = {
  name: bankAccounts.name,
  currency: bankAccounts.currency,
};
export const bankAccountsQuerySchema =
  getpaginationQuerySchema(bankAccountOrderBy);
export const bankAccountsSchema = getListSchema(bankAccountSchema);

export const registerBankAccountRoutes = async (
  fastify: FastifyInstance,
  options: Object
) => {
  create(fastify, options);
  get(fastify, options);
  getAll(fastify, options);
  update(fastify, options);
};
