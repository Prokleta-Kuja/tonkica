export * from "./env";

export const nameof = <T>(name: Extract<keyof T, string>): string => name;
export const sleep = (ms: number) =>
  new Promise((resolve) => setTimeout(resolve, ms));

const precision = 15; // 15 for 64bit, or 6 for 32
const power = Math.pow(10, precision);
