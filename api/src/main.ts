import fastifySwagger from "@fastify/swagger";
import fastify from "fastify";
import {
  fastifyZodOpenApiPlugin,
  fastifyZodOpenApiTransform,
  fastifyZodOpenApiTransformObject,
  serializerCompiler,
  validatorCompiler,
} from "fastify-zod-openapi";
import path from "path";
import { ZodError, z } from "zod";
import { ZodOpenApiVersion } from "zod-openapi";
import { initDb } from "./db";
import { env } from "@utils/env";
import { ensureDevData } from "@db/dev";
import { badRequestSchema } from "./routes/schemas";
import { allRoutes } from "./routes";

export const main = async () => {
  await initDb();

  if (env.IS_DEVELOPMENT) await ensureDevData();

  const app = fastify({ trustProxy: !env.IS_DEVELOPMENT });

  app.setValidatorCompiler(validatorCompiler);
  app.setSerializerCompiler(serializerCompiler);

  const openapi: ZodOpenApiVersion = "3.0.3";

  app.setErrorHandler(function (error, _request, reply) {
    if (error.name === "ZodError")
      error.zodError = error as unknown as ZodError;
    if (error.zodError || error.validation) {
      const res: z.infer<typeof badRequestSchema> = {
        message: "Validation error",
        details: error.zodError.issues.length > 0 ? {} : undefined,
      };
      for (let i = 0; i < error.zodError.issues.length; i++) {
        const issue = error.zodError.issues[i];
        const key = issue.path.join(".");
        if (res.details![key]) res.details![key].push(issue.message);
        else res.details![key] = [issue.message];
      }
      reply.status(400).send(res);
    }
    if (env.IS_DEVELOPMENT) {
      reply.status(500).send({ message: error.message });
      console.error(error.message);
    }
    reply.status(500).send();
  });
  if (env.IS_DEVELOPMENT) {
    await app.register(fastifyZodOpenApiPlugin, { openapi });
    await app.register(fastifySwagger, {
      openapi: {
        info: {
          title: "KennelLink Backend For Frontend",
          version: "1.0.0",
        },
        openapi,
      },
      hideUntagged: true,
      transform: fastifyZodOpenApiTransform,
      transformObject: fastifyZodOpenApiTransformObject,
    });
  }

  await app.register(allRoutes);

  if (!env.IS_DEVELOPMENT) {
    await app.register((childContext, _, done) => {
      childContext.register(import("@fastify/static"), {
        root: path.join(__dirname, "public"),
        wildcard: false,
      });
      childContext.setNotFoundHandler((_, reply) => {
        return reply.code(404).type("text/html").sendFile("index.html");
      });
      done();
    });
  } else {
    await app.register(import("@fastify/swagger-ui"), {
      routePrefix: "/swagger",
    });
    await app.register(import("@fastify/http-proxy"), {
      upstream: "http://localhost:5173",
      websocket: true,
      wsUpstream: "ws://localhost:5173",
    });
  }

  app.listen({ port: 8080 }, (err, address) => {
    if (err) {
      console.error(err);
      process.exit(1);
    }
    console.log(`Server listening at ${address}`);
  });
};
main();
