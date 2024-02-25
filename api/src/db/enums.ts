import { Currency } from "@utils/currencies";
import z from "zod";
import { extendZodWithOpenApi } from "zod-openapi";

extendZodWithOpenApi(z);

export const currencySchema = z.nativeEnum(Currency).openapi({
  ref: "Currency",
  description: "Currency code",
  "x-enum-varnames": Object.keys(Currency).filter((key) => isNaN(Number(key))),
});
