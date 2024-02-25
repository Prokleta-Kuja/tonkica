import { clients, tasks } from "@db/schemas";
import { PgColumn } from "drizzle-orm/pg-core";
import { z } from "zod";
import { extendZodWithOpenApi } from "zod-openapi";
import { getListSchema, getpaginationQuerySchema } from "../schemas";
import { FastifyInstance } from "fastify";

extendZodWithOpenApi(z);
export const taskCreateSchema = z
  .object({
    clientId: z.number(),
    title: z.string().min(2).max(64),
  })
  .openapi({ ref: "TaskCreate" });

export const taskUpdateSchema = z
  .object({
    clientId: z.number(),
    title: z.string().min(2).max(64),
  })
  .openapi({ ref: "TaskUpdate" });

export const taskSchema = z
  .object({
    clientId: z.number(),
    clientName: z.string(),
    title: z.string().min(2).max(64),
    created: z.date(),
  })
  .openapi({ ref: "TaskUpdate" });

export const taskOrderBy = z
  .enum(["client", "title", "created"])
  .openapi({ ref: "TaskOrderBy" });
export const taskOrderByMapping: Record<
  z.infer<typeof taskOrderBy>,
  PgColumn
> = {
  client: clients.name,
  title: tasks.title,
  created: tasks.created,
};
export const tasksQuerySchema = getpaginationQuerySchema(taskOrderBy);
export const tasksSchema = getListSchema(taskSchema);

export const registerTaskRoutes = async (
  fastify: FastifyInstance,
  options: Object
) => {
  getAll(fastify, options);
};
