export * from "./env";

export const nameof = <T>(name: Extract<keyof T, string>): string => name;
export const sleep = (ms: number) =>
  new Promise((resolve) => setTimeout(resolve, ms));
