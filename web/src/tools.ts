export const nameof = <T>(name: Extract<keyof T, string>): string => name

export const dateText = (dateTime: string | null | undefined) => {
  if (!dateTime) return '-'
  const dt = new Date(dateTime)
  return dt.toLocaleString()
}
