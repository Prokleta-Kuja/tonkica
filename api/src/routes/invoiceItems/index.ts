import { invoiceItems } from "@db/schemas";
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
export const invoiceItemCreateSchema = z
  .object({
    title: z.string().min(2).max(64),
    quantity: z.number(),
    price: z.number(),
  })
  .openapi({ ref: "InvoiceItemCreate" });

export const invoiceItemUpdateSchema = z
  .object({
    title: z.string().min(2).max(64),
    quantity: z.number(),
    price: z.number(),
  })
  .openapi({ ref: "InvoiceItemUpdate" });

export const invoiceItemSchema = z
  .object({
    title: z.string().min(2).max(64),
    quantity: z.number(),
    price: z.number(),
  })
  .openapi({ ref: "InvoiceItemUpdate" });

export const invoiceItemOrderBy = z
  .enum(["title", "quantity", "price", "total"])
  .openapi({ ref: "InvoiceItemOrderBy" });
export const invoiceitemOrderByMapping: Record<
  z.infer<typeof invoiceItemOrderBy>,
  PgColumn
> = {
  title: invoiceItems.title,
  quantity: invoiceItems.quantity,
  price: invoiceItems.price,
  total: invoiceItems.total,
};
export const invoiceItemsQuerySchema =
  getpaginationQuerySchema(invoiceItemOrderBy);
export const invoiceItemsSchema = getListSchema(invoiceItemSchema);

export const registerInvoiceItemRoutes = async (
  fastify: FastifyInstance,
  options: Object
) => {
  create(fastify, options);
  get(fastify, options);
  getAll(fastify, options);
  update(fastify, options);
};
