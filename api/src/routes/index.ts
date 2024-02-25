import { FastifyInstance } from "fastify";

const routePrefix = {
  api: "api",
  bankAccounts: "bank-accounts",
} as const;

const params = {
  id: ":id",
} as const;

export const routes = {
  users: `/${routePrefix.api}/${routePrefix.bankAccounts}`,
  user: `/${routePrefix.api}/${routePrefix.bankAccounts}/${params.id}`,
} as const;

export const allRoutes = async (fastify: FastifyInstance, options: Object) => {
  //registerAuthRoutes(fastify, options);
};
