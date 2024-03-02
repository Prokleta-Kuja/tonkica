import { FastifyInstance } from "fastify";
import { registerBankAccountRoutes } from "./bankAccounts";
import { registerClientRoutes } from "./clients";
import { registerInvoiceItemRoutes } from "./invoiceItems";
import { registerInvoiceRoutes } from "./invoices";
import { registerIssuerRoutes } from "./issuers";
import { registerTaskRoutes } from "./tasks";
import { registerTaskTimeRoutes } from "./taskTimes";

export const tags = {
  bankAccount: "BankAccount",
  client: "Client",
  invoice: "Invoice",
  invoiceItem: "InvoiceItem",
  issuer: "Issuer",
  task: "Task",
  taskTime: "TaskTime",
} as const;

const routePrefix = {
  api: "api",
  bankAccounts: "bank-accounts",
  clients: "clients",
  invoices: "invoices",
  issuers: "issuers",
  tasks: "tasks",
} as const;

const params = {
  id: ":id",
  id2: ":id2",
} as const;

export const routes = {
  bankAccounts: `/${routePrefix.api}/${routePrefix.bankAccounts}`,
  bankAccount: `/${routePrefix.api}/${routePrefix.bankAccounts}/${params.id}`,
  clients: `/${routePrefix.api}/${routePrefix.clients}`,
  client: `/${routePrefix.api}/${routePrefix.clients}/${params.id}`,
  invoices: `/${routePrefix.api}/${routePrefix.invoices}`,
  invoice: `/${routePrefix.api}/${routePrefix.invoices}/${params.id}`,
  invoiceItems: `/${routePrefix.api}/${routePrefix.invoices}/${params.id}/items`,
  invoiceItem: `/${routePrefix.api}/${routePrefix.invoices}/${params.id}/items/${params.id2}`,
  issuers: `/${routePrefix.api}/${routePrefix.issuers}`,
  issuer: `/${routePrefix.api}/${routePrefix.issuers}/${params.id}`,
  tasks: `/${routePrefix.api}/${routePrefix.tasks}`,
  task: `/${routePrefix.api}/${routePrefix.tasks}/${params.id}`,
  taskTimes: `/${routePrefix.api}/${routePrefix.tasks}/${params.id}/times`,
  taskTime: `/${routePrefix.api}/${routePrefix.tasks}/${params.id}/times/${params.id2}`,
} as const;

export const allRoutes = async (fastify: FastifyInstance, options: Object) => {
  registerBankAccountRoutes(fastify, options);
  registerClientRoutes(fastify, options)
  registerInvoiceItemRoutes(fastify, options)
  registerInvoiceRoutes(fastify, options)
  registerIssuerRoutes(fastify, options)
  registerTaskRoutes(fastify, options)
  registerTaskTimeRoutes(fastify, options)
};
