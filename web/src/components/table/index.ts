import type { LocationQuery, LocationQueryValue } from 'vue-router'
import Header from './TableHeader.vue'
import Pages from './TablePages.vue'
import Sizes from './TableSizes.vue'
import { nameof } from '@/tools'

export { Header }
export { Pages }
export { Sizes }

export interface IListResponse<T> {
  items: Array<T>
  size: number
  page: number
  total: number
  orderAsc?: 'true'
  orderBy?: string
}

export interface ITableParams<T> {
  size: number
  page: number
  total: number
  orderAsc?: 'true'
  orderBy?: T
}

const pageKey = nameof<ITableParams<unknown>>('page')
const sizeKey = nameof<ITableParams<unknown>>('size')
const totalKey = nameof<ITableParams<unknown>>('total')

const minPageSize = 10,
  defaultPageSize = 25,
  maxPageSize = 100,
  defaultPage = 1
export const defaultPageSizes = [minPageSize, defaultPageSize, 50, maxPageSize]

export const getQueryKey = (key: string, prefix?: string) => (prefix ? `${prefix}.${key}` : key)
export const getParamsKey = (queryKey: string, prefix?: string) =>
  prefix ? queryKey.replace(`${prefix}.`, '') : queryKey

export const initParams = <T>(query?: LocationQuery, prefix?: string): ITableParams<T> => {
  const params: ITableParams<T> = { page: 1, size: 25, total: 0 }
  if (query) {
    const indexableParams = params as { [key: string]: any }
    const trimPrefix = prefix ? `${prefix}.` : ''

    Object.keys(query).forEach((queryKey) => {
      if (!queryKey.startsWith(trimPrefix)) return

      const key = getParamsKey(queryKey, prefix)

      if (query[queryKey] == null) {
        indexableParams[key] = true
        return
      }
      let val = parseInt(query[queryKey]!.toString())
      if (isNaN(val)) val = parseFloat(query[queryKey]!.toString())
      if (!isNaN(val)) {
        indexableParams[key] = val
        return
      }

      indexableParams[key] = query[queryKey]!.toString()
    })
  }

  if (sizeKey in params) {
    const indexableParams = params as { [key: string]: any }
    const val = parseInt(indexableParams[sizeKey])
    if (isNaN(val)) indexableParams[sizeKey] = defaultPageSize
    else if (val < minPageSize) indexableParams[sizeKey] = minPageSize
    else if (val > maxPageSize) indexableParams[sizeKey] = maxPageSize
    else if (!defaultPageSizes.some((size) => size === val))
      indexableParams[sizeKey] = defaultPageSize
  }

  return params
}

export const getQuery = <T>(
  current: Record<string, LocationQueryValue | LocationQueryValue[]>,
  params: ITableParams<T>,
  prefix?: string
): LocationQuery => {
  const query: Record<string, any | null> = {}
  const indexableParams = params as Record<string, any>
  Object.keys(params).forEach((key) => {
    const queryKey = getQueryKey(key, prefix)
    if (key === totalKey) return
    if (key === pageKey && indexableParams[key] === defaultPage) {
      if (current[queryKey]) query[queryKey] = defaultPage.toString()
      return
    }
    if (key === sizeKey && indexableParams[key] === defaultPageSize) {
      if (current[queryKey]) query[queryKey] = defaultPageSize.toString()
      return
    }

    const type = typeof indexableParams[key]
    if (type === 'boolean')
      if (indexableParams[key]) {
        query[queryKey] = null
        return
      } else return
    else if (indexableParams[key] === '') {
      if (current[queryKey]) query[queryKey] = undefined
      return
    }

    query[queryKey] = indexableParams[key]
  })

  return { ...current, ...query }
}

export const updateParams = <T>(
  params: ITableParams<T>,
  response: {
    size: number
    page: number
    total: number
    orderAsc: boolean
    orderBy?: T | null
  }
) => {
  if (params.size !== response.size) params.size = response.size
  if (params.page !== response.page) params.page = response.page
  if (params.total !== response.total) params.total = response.total
  if (Boolean(params.orderAsc) !== response.orderAsc)
    params.orderAsc = response.orderAsc ? 'true' : undefined
  if (params.orderBy !== response.orderBy)
    params.orderBy = response.orderBy === null ? undefined : response.orderBy
}
