import type { ValidationError } from '@/api'

export default interface IModelState<T> {
  loading?: boolean
  submitting?: boolean
  model: T
  error?: ValidationError
}