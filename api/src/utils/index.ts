export * from "./env";

export const nameof = <T>(name: Extract<keyof T, string>): string => name;