/**
 * Type Re-exports
 *
 * This file provides backward compatibility for components
 * using the old type locations.
 */

// Re-export with alias for legacy code
export type { ApartmentResponse as Apartment } from "./api/types"
export type { ApartmentFilterParams as ApartmentFilters } from "./api/types"

// Re-export all API types
export * from "./api/types"
