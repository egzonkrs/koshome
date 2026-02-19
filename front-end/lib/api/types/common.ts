/**
 * Common API Types
 * Shared types used across all API modules.
 */

export interface PaginationMetadata {
  pageNumber: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasNextPage: boolean
  hasPreviousPage: boolean
  // Optional fields (backend may not send all of these)
  currentPageSize?: number
  isFirstPage?: boolean
  isLastPage?: boolean
  firstCursor?: string
  lastCursor?: string
}

export interface PaginatedResponse<T> {
  items: T[] | null
  pagination: PaginationMetadata
}

export interface ApiResponse<T> {
  data: T | null
  isFailed: boolean
  isSuccess: boolean
  reasons?: Record<string, string | null> | null
  errors?: Record<string, string | null> | null
}

export interface ApiError {
  code: string
  message: string
  field?: string
}
