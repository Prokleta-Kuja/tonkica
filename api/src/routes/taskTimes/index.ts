import { taskTimes, tasks } from "@db/schemas";
import { PgColumn } from "drizzle-orm/pg-core";
import { z } from "zod";
import { extendZodWithOpenApi } from "zod-openapi";
import { getListSchema, getpaginationQuerySchema } from "../schemas";
import { FastifyInstance } from "fastify";
import { create } from "./create";
import { getAll } from "./getAll";
import { update } from "./update";

extendZodWithOpenApi(z);
export const taskTimeCreateSchema = z
  .object({
    start: z.date(),
    durationMs: z.number().multipleOf(1, "Must be an integer"),
  })
  .openapi({ ref: "TaskTimeCreate" });

export const taskTimeUpdateSchema = z
  .object({
    start: z.date(),
    durationMs: z.number().multipleOf(1, "Must be an integer"),
  })
  .openapi({ ref: "TaskTimeUpdate" });

export const taskTimeSchema = z
  .object({
    taskTitle: z.string(),
    start: z.date(),
    durationMs: z.number().multipleOf(1, "Must be an integer"),
  })
  .openapi({ ref: "TaskTimeUpdate" });

export const taskTimeOrderBy = z
  .enum(["start", "duration", "task"])
  .openapi({ ref: "TaskTimeOrderBy" });
export const tasktimeOrderByMapping: Record<
  z.infer<typeof taskTimeOrderBy>,
  PgColumn
> = {
  start: taskTimes.start,
  duration: taskTimes.durationMs,
  task: tasks.title,
};
export const taskTimesQuerySchema = getpaginationQuerySchema(taskTimeOrderBy);
export const taskTimesSchema = getListSchema(taskTimeSchema);

export const registerTaskTimeRoutes = async (
  fastify: FastifyInstance,
  options: Object
) => {
  create(fastify, options);
  getAll(fastify, options);
  update(fastify, options);
};
