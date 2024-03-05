export * from "./env";

export const nameof = <T>(name: Extract<keyof T, string>): string => name;
export const sleep = (ms: number) =>
  new Promise((resolve) => setTimeout(resolve, ms));
export const dbRound = (num: number) => Number(num.toFixed(6));
export const dbRoundSafe = (num?: number) => {
  if (num) return Number(num.toFixed(6));
};
