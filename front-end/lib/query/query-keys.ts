/**
 * TanStack Query Keys
 *
 * Centralized key factory for cache management.
 */

import type { ApartmentFilterParams } from "../api/types"

export const queryKeys = {
  apartments: {
    all: ["apartments"] as const,
    lists: () => [...queryKeys.apartments.all, "list"] as const,
    list: (params: ApartmentFilterParams) => [...queryKeys.apartments.lists(), params] as const,
    details: () => [...queryKeys.apartments.all, "detail"] as const,
    detail: (id: string) => [...queryKeys.apartments.details(), id] as const,
  },

  auth: {
    all: ["auth"] as const,
    user: () => [...queryKeys.auth.all, "user"] as const,
  },

  locations: {
    all: ["locations"] as const,
    cities: () => [...queryKeys.locations.all, "cities"] as const,
    countries: () => [...queryKeys.locations.all, "countries"] as const,
  },
} as const
