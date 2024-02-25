import { customType } from "drizzle-orm/pg-core";

export const numericNumber = customType<{ data: number }>({
  dataType() {
    return "numeric(15, 6)";
  },
  fromDriver(value) {
    return Number(value);
  },
});
