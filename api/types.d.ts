import fastify from "fastify";
import * as http from "http";
import { ZodError } from "zod";

declare module "fastify" {
  export interface FastifyError {
    zodError: ZodError;
  }
  export interface FastifyRequest {
    params: { id?: string };
  }
}
