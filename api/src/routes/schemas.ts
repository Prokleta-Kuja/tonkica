import z from "zod";
import { extendZodWithOpenApi } from "zod-openapi";

extendZodWithOpenApi(z);
export const idParamSchema = z.object({
  id: z.number(),
});
export const id2ParamSchema = idParamSchema.extend({
  id2: z.number(),
});

export const getpaginationQuerySchema = <T extends z.ZodTypeAny>(
  orderBySchema: T
) =>
  z.object({
    page: z.coerce.number().min(1).default(1).catch(1),
    size: z.coerce.number().min(10).max(100).default(25).catch(25),
    orderBy: orderBySchema.nullish(),
    orderAsc: z
      .literal("true")
      .optional()
      .pipe(z.coerce.boolean().default(false)),
    term: z.string().nullish(),
  });

export const notFoundSchema = z
  .object({
    error: z.string(),
  })
  .openapi({ ref: "NotFound" });

export const badRequestSchema = z
  .object({
    message: z.string(),
    details: z.record(z.string(), z.array(z.string())).optional(),
  })
  .openapi({ ref: "BadRequest" });

export const emptySchema = {};

export const listSchema = z.object({
  page: z.number().min(1).default(1).catch(1),
  size: z.number().min(10).max(100).default(25).catch(25),
  orderBy: z.string().nullish(),
  orderAsc: z.boolean().default(false).catch(false),
});

export const getListSchema = <T extends z.ZodTypeAny>(itemSchema: T) =>
  z.object({
    page: z.number().min(1).default(1).catch(1),
    size: z.number().min(10).max(100).default(25).catch(25),
    orderBy: z.string().nullish(),
    orderAsc: z.boolean().default(false).catch(false),
    total: z.number().min(0).default(0),
    items: z.array(itemSchema),
  });
