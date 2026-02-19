/**
 * Location Query Hooks
 */

import { useQuery } from "@tanstack/react-query"
import { queryKeys } from "./query-keys"
import { locationProvider } from "../api/providers"

export function useCities() {
  return useQuery({
    queryKey: queryKeys.locations.cities(),
    queryFn: () => locationProvider.getCities(),
    staleTime: 1000 * 60 * 30,
  })
}

export function useCountries() {
  return useQuery({
    queryKey: queryKeys.locations.countries(),
    queryFn: () => locationProvider.getCountries(),
    staleTime: 1000 * 60 * 30,
  })
}
